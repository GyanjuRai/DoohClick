
CREATE OR ALTER FUNCTION tf_campaign_flight_screen()
RETURNS TABLE
AS
RETURN
(
	 SELECT      cf.campaign_id,
                (SELECT     cf2.id,
                            cf2.campaign_id,
                            cf2.start_date,
                            cf2.end_date,
                            (SELECT     cfs.id,
                                        cfs.campaign_flight_id,
                                        cfs.screen_id
                             FROM       dbo.campaign_flight_screen cfs
                             WHERE      cfs.campaign_flight_id = cf2.id
                             AND        cfs.is_deleted = 0
                             FOR JSON PATH, INCLUDE_NULL_VALUES) AS screens
                 FROM       dbo.campaign_flight cf2
                 WHERE      cf2.campaign_id = cf.campaign_id
                 AND        cf2.is_deleted = 0
                 FOR JSON PATH, INCLUDE_NULL_VALUES) AS campaign_flight
    FROM        dbo.campaign_flight cf
    WHERE       cf.is_deleted = 0
    GROUP BY    cf.campaign_id
);