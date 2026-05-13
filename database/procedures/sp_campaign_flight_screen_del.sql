/*

=========================================================================================
- Description: Soft delete campaign flight screen. Does not contain transaction handle by
                it's parent.
- Author: Gyanju Rai
- Created: 2026-05-08
==========================================================================================

DECLARE @Json NVARCHAR(MAX) = N'[
                                  {
                                    "Id": 1,
                                    "DeletedBy": 1
                                  }
                                ]';

EXEC dbo.sp_campaign_flight_screen_del @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_campaign_flight_screen_del
(
    @Json NVARCHAR(MAX) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON
    BEGIN TRY
        CREATE TABLE #campaign_flight_screen
        (
            id INT NOT NULL,
            deleted_by INT
        );

        INSERT INTO #campaign_flight_screen
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

        UPDATE  cfs
        SET cfs.is_deleted = 1,
            cfs.deleted_by = tcfs.deleted_by,
            cfs.deleted_at = GETUTCDATE()
        FROM dbo.campaign_flight_screen AS cfs
        INNER JOIN #campaign_flight_screen AS tcfs ON cfs.id = tcfs.id;

        SELECT @Json = ISNULL((
                SELECT  cfs.*
                FROM dbo.campaign_flight_screen AS cfs
                INNER JOIN #campaign_flight_screen AS tcf ON cfs.id = tcf.id
                FOR JSON PATH,
                INCLUDE_NULL_VALUES
        ), '[]');

        DROP TABLE IF EXISTS #campaign_flight_screen;

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END