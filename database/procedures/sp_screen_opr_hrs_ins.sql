
/*

========================================
- Description: insert operating hours 
- Author: Gyanju Rai
- Created: 2026-04-24
========================================

DECLARE @Json NVARCHAR(MAX) = N'[
                                  
                                    {
                                        "Id": 1,
                                        "ScreenId": 1,
                                        "DayOfWeek": "SUNDAY",
                                        "OpenTime": "06:00",
                                        "CloseTime": "23:00",
                                        "AudienceSource": "FOOTTRAFFICSENSOR",
                                        "EstimatedImpression": 5000000,
                                        "CreatedBy": 1,
                                        "DeletedBy": 1
                                    },
                                    {
                                        "Id": 2,
                                        "ScreenId": 1,
                                        "DayOfWeek": "SATURDAY",
                                        "OpenTime": "08:00",
                                        "CloseTime": "00:00",
                                        "AudienceSource": "FOOTTRAFFICSENSOR",
                                        "EstimatedImpression": 7500000,
                                        "CreatedBy": 1,
                                        "DeletedBy": 1
                                    },
                                    {
                                        "Id": null,
                                        "ScreenId": 1,
                                        "DayOfWeek": "SUNDAY",
                                        "OpenTime": "06:00",
                                        "CloseTime": "23:00",
                                        "AudienceSource": "FOOTTRAFFICSENSOR",
                                        "EstimatedImpression": 5000000,
                                        "CreatedBy": 1,
                                        "DeletedBy": null
                                    },
                                    {
                                        "Id": null,
                                        "ScreenId": 1,
                                        "DayOfWeek": "SATURDAY",
                                        "OpenTime": "08:00",
                                        "CloseTime": "00:00",
                                        "AudienceSource": "FOOTTRAFFICSENSOR",
                                        "EstimatedImpression": 7500000,
                                        "CreatedBy": 1,
                                        "DeletedBy": null
                                    }
                                    
                                ]';

EXEC inv.sp_screen_opr_hrs_tsk @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE inv.sp_screen_opr_hrs_tsk
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
    SET NOCOUNT ON

    BEGIN TRY
        
        DECLARE @TRANCOUNT INT = 0;

        CREATE TABLE #inserted
        (
            id INT NOT NULL
        );

        CREATE TABLE #screen_opr_hrs
        (
            id INT NULL,
            screen_id INT NOT NULL,
            day_of_week NVARCHAR(50) NOT NULL,
            open_time TIME NOT NULL,
            close_time TIME NOT NULL,
            audience_source NVARCHAR(50) NOT NULL,
            estimated_impression INT NULL,
            created_by INT NOT NULL,
            deleted_by INT NULL,
            created_at DATETIME2 NOT NULL
        );

        INSERT INTO #screen_opr_hrs
        (
            id,
            screen_id,
            day_of_week,
            open_time,
            close_time,
            audience_source,
            estimated_impression,
            created_by,
            deleted_by,
            created_at
        )
        SELECT  oj.Id,
                oj.ScreenId,
                oj.[DayOfWeek],
                oj.OpenTime,
                oj.CloseTime,
                oj.AudienceSource,
                oj.EstimatedImpression,
                oj.CreatedBy,
                oj.DeletedBy,
                GETUTCDATE()
        FROM OPENJSON(@Json)
        WITH
        (   
            Id INT,
            ScreenId INT,
            [DayOfWeek] NVARCHAR(50),
            OpenTime TIME,
            CloseTime TIME,
            AudienceSource NVARCHAR(50),
            EstimatedImpression INT,
            CreatedBy INT,
            DeletedBy INT
        ) AS oj;

        IF @@TRANCOUNT = 0
        BEGIN
            SET @TRANCOUNT = 1;
            BEGIN TRANSACTION
        END
        
        UPDATE soh
        SET soh.is_deleted = 1,
            soh.deleted_by = tsoh.created_by,
            soh.deleted_at = GETUTCDATE()
        FROM inv.screen_operating_hour AS soh
        INNER JOIN #screen_opr_hrs AS tsoh ON soh.id = tsoh.id
        AND tsoh.deleted_by IS NOT NULL;

        INSERT INTO inv.screen_operating_hour
        (   
            screen_id,
            day_of_week,
            open_time,
            close_time,
            audience_source,
            estimated_impression,
            created_by,
            created_at
        )
        OUTPUT Inserted.id INTO #inserted ( id )
        SELECT  tsoh.screen_id,
                tsoh.day_of_week,
                tsoh.open_time,
                tsoh.close_time,
                tsoh.audience_source,
                tsoh.estimated_impression,
                tsoh.created_by,
                tsoh.created_at
        FROM #screen_opr_hrs AS tsoh
        WHERE tsoh.id IS NULL;

        IF @TRANCOUNT > 0
        BEGIN
            SET @TRANCOUNT = 0;
            COMMIT TRANSACTION;
        END

        SELECT @Json = ISNULL((
			SELECT  soh.id,
				    soh.screen_id,
				    soh.day_of_week,
				    soh.open_time,
				    soh.close_time,
				    soh.audience_source,
				    soh.estimated_impression,
				    soh.created_by,
				    soh.created_at
			FROM inv.screen_operating_hour AS soh
            INNER JOIN #screen_opr_hrs AS tsoh ON soh.id = tsoh.id
			OR EXISTS (
					SELECT 1
					FROM #inserted
					WHERE id = soh.id
					)
			FOR JSON PATH,
				INCLUDE_NULL_VALUES
			), '[]');

        DROP TABLE #inserted, #screen_opr_hrs;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 AND @TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DROP TABLE IF EXISTS #inserted, #screen_opr_hrs;
        THROW;
    END CATCH
END