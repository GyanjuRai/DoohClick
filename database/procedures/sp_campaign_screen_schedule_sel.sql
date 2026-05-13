
/*

=====================================================================
- Description: retrieve screen record with its
               operating hours and supported media for grid.
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                    "Id": 2
                                }';

EXEC dbo.sp_campaign_screen_schedule_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_campaign_screen_schedule_sel
(
    @Json NVARCHAR(MAX)    
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CampaignId INT = JSON_VALUE(@Json, '$.Id');
   
   WITH campaign_flights AS (
    SELECT 
        cf.id,
        cf.start_date,
        cf.end_date
    FROM dbo.campaign AS c
    INNER JOIN dbo.campaign_flight AS cf ON c.id = cf.campaign_id
    WHERE c.id = @CampaignId AND cf.is_deleted = 0
    )
    SELECT ISNULL((SELECT
        cf.id                       AS flight_id,
        cfs.id                      AS campaign_flight_screen_id,
        cf.[start_date],
        cf.end_date,

        s.id                        AS screen_id,
        s.[name]                     AS screen_name,
        s.default_resolution        AS screen_resolution,
        s.country_code              AS screen_country,
        s.city                      AS screen_city,

        ISNULL(
            JSON_QUERY((
                SELECT
                    css.id,
                    css.start_time,
                    css.end_time,
                    css.day_of_week,

                    ISNULL(
                        JSON_QUERY((
                            SELECT
                                cpi.id,
                                cpi.duration_seconds,
                                cpi.media_id,
                                cpi.play_order,
                                ml.display_name,
                                ml.file_url,
                                ml.file_size_bytes
                            FROM dbo.campaign_playlist_item AS cpi
                            INNER JOIN dbo.media_library AS ml ON cpi.media_id = ml.id
                            WHERE   cpi.schedule_id = css.id AND 
                                    cpi.is_deleted = 0 AND
                                    ml.is_deleted = 0
                            FOR JSON PATH,
                            INCLUDE_NULL_VALUES
                        )),
                    '[]') AS playlist

                FROM dbo.campaign_screen_schedule AS css
                WHERE   css.campaign_flight_screen_id = cfs.id AND
                        css.is_deleted = 0
                FOR JSON PATH,
                INCLUDE_NULL_VALUES
            )),
        '[]') AS campaign_screen_schedules
    FROM campaign_flights AS cf
    INNER JOIN dbo.campaign_flight_screen AS cfs ON cfs.campaign_flight_id = cf.id
    INNER JOIN inv.screen AS s ON cfs.screen_id = s.id
    FOR JSON PATH, INCLUDE_NULL_VALUES), '[]');
   
END