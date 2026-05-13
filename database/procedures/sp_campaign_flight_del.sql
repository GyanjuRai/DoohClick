
/*

=========================================================================================
- Description: Soft delete campaign flight. Transaction is control by the parent sp calling
                it.
- Author: Gyanju Rai
- Created: 2026-05-08
==========================================================================================

DECLARE @Json NVARCHAR(MAX) = N'[
                                  {
                                    "Id": 2,
                                    "DeletedBy": 1
                                  }
                                ]';

EXEC dbo.sp_campaign_flight_del @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_campaign_flight_del
(
    @Json NVARCHAR(MAX) OUTPUT
)
AS
BEGIN
SET NOCOUNT ON
    BEGIN TRY

    CREATE TABLE #campaign_flight
    (
        id INT, 
        deleted_by INT
    );

    INSERT INTO #campaign_flight
    (
        id,
        deleted_by
    )
    SELECT  oj.Id,
            oj.DeletedBy
    FROM OPENJSON(@Json)
    WITH
    (
        Id INT,
        DeletedBy INT
    ) AS oj;

    UPDATE  cf
    SET cf.is_deleted = 1,
        cf.deleted_by = tcf.deleted_by,
        cf.deleted_at = GETUTCDATE()
    FROM dbo.campaign_flight AS cf
    INNER JOIN #campaign_flight AS tcf ON cf.id = tcf.id;

    SELECT @Json = ISNULL((
                SELECT  cf.*
                FROM dbo.campaign_flight AS cf
                INNER JOIN #campaign_flight AS tcf ON cf.id = tcf.id
                FOR JSON PATH,
                INCLUDE_NULL_VALUES
            ), '[]');

    DROP TABLE IF EXISTS #campaign_flight;
    END TRY
    BEGIN CATCH

        THROW
    END CATCH
END