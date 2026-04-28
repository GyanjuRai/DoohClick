
/*

=====================================================================
- Description: Soft delete screen record with its
               operating hours and supported media.
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                 "Id": 1,
                                 "TenantId": 1,
                                 "DeletedBy": 1
                                }';

EXEC inv.sp_screen_del @Json = @Json OUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE inv.sp_screen_del
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
    SET NOCOUNT ON
    BEGIN TRY

    DECLARE @HasActiveCampaign BIT = 0;
    
    CREATE TABLE #screen
    (
        id                  INT NOT NULL,
        tenant_id           INT NOT NULL,
        deleted_by          INT NOT NULL,
        deleted_at          DATETIME2 DEFAULT GETUTCDATE()
    );

    INSERT INTO #screen 
    (
        id, 
        tenant_id, 
        deleted_by
    )
    SELECT  Id, 
            TenantId, 
            DeletedBy
    FROM OPENJSON(@Json)
    WITH
    (
        Id         INT,
        TenantId   INT,
        DeletedBy  INT
    );

    SELECT @HasActiveCampaign = 1 
    FROM inv.screen AS s
    INNER JOIN #screen AS ts ON s.id = ts.id
    INNER JOIN dbo.campaign_schedule AS cs ON s.id = cs.screen_id
    INNER JOIN dbo.campaign AS c ON cs.campaign_id = c.id
    WHERE  s.tenant_id   = ts.tenant_id
    AND    c.[status] IN ('SCHEDULED', 'ACTIVE');

    IF  @HasActiveCampaign = 1
        THROW 50400, 'Screen has active campaigns.', 1;
    
    BEGIN TRANSACTION

        UPDATE s
        SET    s.is_deleted = 1,
                s.deleted_by = ts.deleted_by,
                s.deleted_at = ts.deleted_at
        FROM   inv.screen AS s
        INNER JOIN #screen AS ts ON s.id = ts.id;

        UPDATE oh
        SET oh.is_deleted = 1,
            oh.deleted_by = ts.deleted_by,
            oh.deleted_at = ts.deleted_at
        FROM inv.screen_operating_hour AS oh
        INNER JOIN #screen AS ts ON oh.screen_id = ts.id;

        UPDATE sm
        SET sm.is_deleted = 1,
            sm.deleted_by = ts.deleted_by,
            sm.deleted_at = ts.deleted_at
        FROM inv.screen_supported_media AS sm 
        INNER JOIN #screen AS ts ON sm.screen_id = ts.id;
        
    COMMIT TRANSACTION

    SELECT @Json = ISNULL((
                        SELECT  s.id,
                                s.tenant_id,
                                t.[name]        AS tenant_name,
                                s.[name],
                                s.normalized_name,
                                s.screen_code,
                                s.[description],
                                s.default_resolution,
                                s.orientation,
                                s.[location],
                                s.address_line,
                                JSON_QUERY(s.tag) AS tag,
                                s.country_code,
                                s.city,
                                s.timezone,
                                s.is_active,
                                s.rate_per_hour,
                                s.currency,
                                JSON_QUERY(oh.operating_hour) AS operating_hour,
                                JSON_QUERY(sm.supported_media) AS supported_media,
                                s.created_by,
                                CONCAT(cu.[name], ' ', ISNULL(cu.sur_name, '')) AS creator,
                                s.updated_by,
                                CONCAT(uu.[name], ' ', ISNULL(uu.sur_name, '')) AS modifier,
                                s.created_at,
                                s.updated_at,
                                s.is_deleted,
                                CONCAT(uu.[name], ' ', ISNULL(uu.sur_name, '')) AS deletor,
                                s.deleted_at
                        FROM inv.screen AS s
                        INNER JOIN #screen AS ts ON s.id = ts.id
                        INNER JOIN [identity].tenant     AS t  ON s.tenant_id   = t.id
                        INNER JOIN [identity].[user]     AS cu ON s.created_by  = cu.id
                        LEFT  JOIN [identity].[user]     AS uu ON s.updated_by  = uu.id
                        LEFT JOIN inv.tf_screen_operating_hour() AS oh ON s.id = oh.screen_id
                        LEFT JOIN inv.tf_screen_supported_media() AS sm On s.id = sm.screen_id
                        FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
                        ), '{}');

    DROP TABLE #screen;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION

        DROP TABLE IF EXISTS #screen;
        THROW;
    END CATCH
END