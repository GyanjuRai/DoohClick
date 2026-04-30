
CREATE OR ALTER FUNCTION inv.tf_screen_supported_media()
RETURNS TABLE
AS
RETURN
(
	SELECT
            screen_id,
            (
                SELECT sm2.id, sm2.media_type
                FROM inv.screen_supported_media sm2
                WHERE sm2.screen_id = sm.screen_id AND sm2.is_deleted = 0
                FOR JSON PATH, INCLUDE_NULL_VALUES
            ) AS supported_media
        FROM inv.screen_supported_media sm
        GROUP BY screen_id
);