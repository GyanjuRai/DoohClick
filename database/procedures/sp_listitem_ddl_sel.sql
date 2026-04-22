
/*

=====================================================================
- Description: Retrieve listitem for dropdown using category name
- Author: Gyanju Rai
- Created: 2026-04-05
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'
								{
									"Category": "player_status "
								}';

EXEC shared.sp_listitem_ddl_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE shared.sp_listitem_ddl_sel
(
	@Json AS NVARCHAR(MAX)
)
AS
BEGIN
	
	DECLARE @category NVARCHAR(100) = LTRIM(RTRIM(ISNULL(JSON_VALUE(@Json, '$.Category'), '')));

	SELECT	li.id,
			li.item,
			li.code
	FROM shared.list_item AS li
	INNER JOIN shared.list_item_category AS lic ON li.category_id = lic.id
	WHERE lic.category = @category FOR JSON PATH;

END;