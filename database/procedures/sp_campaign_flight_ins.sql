
/*

=========================================================================================
- Description: Insert campaign flight. Transaction is control by the parent sp calling
                it.
- Author: Gyanju Rai
- Created: 2026-05-08
==========================================================================================

DECLARE @Json NVARCHAR(MAX) = N'[
                                  {
                                    "CampaignId": 2,
                                    "StartDate": "2026-06-01",
                                    "EndDate": "2026-06-15"
                                  },
                                  {
                                    "CampaignId": 2,
                                    "StartDate": "2026-06-16",
                                    "EndDate": "2026-06-30"
                                  }
                                ]';

EXEC dbo.sp_campaign_flight_ins @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_campaign_flight_ins
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
SET NOCOUNT ON
    BEGIN TRY
        
        CREATE TABLE #inserted
        (
            id INT NOT NULL
        );

        CREATE TABLE #campaign_flight
        (
            campaign_id INT NOT NULL,
            [start_date] DATE NOT NULL,
	        end_date DATE NOT NULL
        );

        INSERT INTO #campaign_flight
        (
            campaign_id,
            [start_date],
            end_date
        )
        SELECT  oj.CampaignId,
                oj.StartDate,
                oj.EndDate
        FROM OPENJSON(@Json)
        WITH
        (
            CampaignId  INT,
            StartDate   DATE,
            EndDate     DATE
        ) AS oj;

        INSERT INTO dbo.campaign_flight
        (
            campaign_id,
            [start_date],
            end_date
        )
        OUTPUT Inserted.id INTO #inserted (id)
        SELECT  tcf.campaign_id,
                tcf.[start_date],
                tcf.end_date
        FROM #campaign_flight AS tcf;
        
        SELECT @Json = ISNULL((
            SELECT  *
            FROM dbo.campaign_flight AS cf
            WHERE EXISTS (
                SELECT 11
                FROM #inserted WHERE id = cf.id
            )
            FOR JSON PATH,
            INCLUDE_NULL_VALUES
        ), '[]');

        DROP TABLE IF EXISTS #campaign_flight, #inserted;
    END TRY
    BEGIN CATCH

        THROW;

    END CATCH
END