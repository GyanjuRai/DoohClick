
CREATE OR ALTER FUNCTION inv.tf_screen_operating_hour()
RETURNS TABLE
AS
RETURN
(
	SELECT 
            screen_id,
            (
                SELECT  oh2.id, 
                        oh2.day_of_week, 
                        oh2.open_time, 
                        oh2.close_time, 
                        oh2.audience_source, 
                        oh2.estimated_impression
                FROM inv.screen_operating_hour oh2
                WHERE oh2.screen_id = oh.screen_id
                FOR JSON PATH, INCLUDE_NULL_VALUES
            ) AS operating_hour
        FROM inv.screen_operating_hour oh
        GROUP BY screen_id
);