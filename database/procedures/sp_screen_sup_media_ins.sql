

/*

=====================================================================
- Description: Upsert a screen record with its
               operating hours and supported media
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'[
                                  {
                                    "Id": null,
                                    "ScreenId": 1,
                                    "MediaType": "MP4",
                                    "CreatedBy": 1,
                                    "DeletedBy": null
                                  },
                                  {
                                    "Id": null,
                                    "ScreenId": 1,
                                    "MediaType": "JPEG",
                                    "CreatedBy": 1,
                                    "DeletedBy": null
                                  }
                                ]';

EXEC inv.sp_screen_sup_media_tsk @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE inv.sp_screen_sup_media_tsk
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @TRANCOUNT INT = 0;

        CREATE TABLE #inserted
        (
            id INT NOT NULL
        );

        CREATE TABLE #screen_sup_media
        (
            id INT NULL,
            screen_id INT NOT NULL,
            media_type NVARCHAR(50) NOT NULL,
            created_by INT NOT NULL,
            deleted_by INT NULL,
            created_at DATETIME2 NOT NULL
        );

        INSERT INTO #screen_sup_media
        (
            id,
            screen_id,
            media_type,
            created_by,
            deleted_by,
            created_at
        )
        SELECT  oj.Id,
                oj.ScreenId,
                oj.MediaType,
                oj.CreatedBy,
                oj.DeletedBy,
                GETUTCDATE()
        FROM OPENJSON(@Json)
        WITH
        (
            Id INT,
            ScreenId INT,
            MediaType NVARCHAR(50),
            DeletedBy INT,
            CreatedBy INT
        ) AS oj;

        IF @@TRANCOUNT = 0
        BEGIN
            SET @TRANCOUNT = 1;
            BEGIN TRANSACTION
        END
        
        UPDATE ssm
        SET ssm.is_deleted = 1,
            ssm.deleted_by = tssm.deleted_by,
            ssm.deleted_at = GETUTCDATE()
        FROM inv.screen_supported_media AS ssm
        INNER JOIN #screen_sup_media AS tssm ON ssm.id = tssm.id
        AND tssm.deleted_by IS NOT NULL;

        INSERT INTO inv.screen_supported_media
        (
            screen_id,
            media_type,
            created_by,
            created_at
        )
        OUTPUT Inserted.id INTO #inserted ( id )
        SELECT  tssm.screen_id,
                tssm.media_type,
                tssm.created_by,
                tssm.created_at
        FROM #screen_sup_media AS tssm
        WHERE id IS NULL;

        IF @TRANCOUNT > 0
        BEGIN
            SET @TRANCOUNT = 0;
            COMMIT TRANSACTION;
        END

        SELECT @Json = ISNULL((SELECT  ssm.id,
                                        ssm.screen_id,
                                        ssm.media_type,
                                        ssm.created_by,
                                        ssm.created_at
        FROM inv.screen_supported_media AS ssm
        INNER JOIN #screen_sup_media AS tssm ON ssm.id = tssm.id
        OR EXISTS
        (
            SELECT  1
            FROM #inserted
            WHERE id = ssm.id
        ) FOR JSON PATH, INCLUDE_NULL_VALUES),
        '[]');

        DROP TABLE #inserted, #screen_sup_media;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND @TRANCOUNT > 0
            ROLLBACK TRANSACTION

        DROP TABLE IF EXISTS #inserted, #screen_sup_media;
        THROW;
    END CATCH
END