
/*

=========================================================================================
- Description: Insert campaign flight screen. Does not contain transaction handle by
                it's parent.
- Author: Gyanju Rai
- Created: 2026-05-08
==========================================================================================

DECLARE @Json NVARCHAR(MAX) = N'[
                                  {
                                    "CampaignFlightId": 42,
                                    "ScreenId": 1
                                  },
                                  {
                                    "CampaignFlightId": 42,
                                    "ScreenId": 2
                                  },
                                  {
                                    "CampaignFlightId": 1,
                                    "ScreenId": 1
                                  }
                                ]';

EXEC dbo.sp_campaign_flight_screen_ins @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_campaign_flight_screen_ins
(
    @Json NVARCHAR(MAX) OUTPUT
)
AS
BEGIN
    BEGIN TRY
       SET NOCOUNT ON; 
        CREATE TABLE #inserted
        (
            id INT NOT NULL
        );

        CREATE TABLE #campaing_flight_screen
        (
            campaign_flight_id INT,
            screen_id INT
        );

        INSERT INTO #campaing_flight_screen
        (
            campaign_flight_id,
            screen_id
        )
        SELECT  oj.CampaignFlightId,
                oj.ScreenId
        FROM OPENJSON(@Json)
        WITH (
            CampaignFlightId INT,
            ScreenId INT
        ) AS oj;

        INSERT INTO dbo.campaign_flight_screen
        (
            campaign_flight_id,
            screen_id
        )
        OUTPUT Inserted.id INTO #inserted ( id )
        SELECT  campaign_flight_id,
                screen_id
        FROM #campaing_flight_screen;

        SELECT @Json = ISNULL((
                SELECT  *
                FROM dbo.campaign_flight_screen AS cfs
                WHERE EXISTS (
                    SELECT 1
                    FROM #inserted WHERE id = cfs.id
                )
                FOR JSON PATH,
                INCLUDE_NULL_VALUES
        ), '[]');

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END