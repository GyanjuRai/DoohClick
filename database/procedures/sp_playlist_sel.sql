
/*

=====================================================================
- Description: Upsert a screen record with its
               operating hours and supported media
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                    "At": "2026-05-14T10:30:00Z",
                                    "ScreenId": 2
                                }';

EXEC dbo.sp_playlist_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_playlist_sel
(
    @Json NVARCHAR(MAX)
)
AS
BEGIN
    
    DECLARE @DateTime DATETIMEOFFSET = JSON_VALUE(@Json, '$.At');

    SELECT LOWER(DATENAME(DW, @DateTime));

    SELECT shared.sf_get_listitem_by_code('THU');

END;