
CREATE OR ALTER FUNCTION tf_campaign_flight_screen()
RETURNS TABLE
AS
RETURN
(
	SELECT      cf.campaign_id,
                (SELECT     cfs2.id,
                            cfs2.campaign_flight_id,
                            cfs2.screen_id
                 FROM       dbo.campaign_flight_screen cfs2
                 INNER JOIN dbo.campaign_flight cf2 ON cf2.id = cfs2.campaign_flight_id
                 WHERE      cf2.campaign_id = cf.campaign_id
                 AND        cfs2.is_deleted = 0
                 FOR JSON PATH, INCLUDE_NULL_VALUES) AS campaign_flight_screen
    FROM        dbo.campaign_flight cf
    WHERE       cf.is_deleted = 0
    GROUP BY    cf.campaign_id
);