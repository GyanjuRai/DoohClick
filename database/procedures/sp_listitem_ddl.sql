
/*

======================================================================
- Description: Retrieve listitem for dropdown using category name
- Author: Gyanju Rai
- Created: 2026-04-05
======================================================================

======================================================================
- Modified At: 2026-04-30
- Modifier: Gyanju Rai
- Description: Changed Filter listitem from listitem_category.category
				to listitem_category.category_code
======================================================================

DECLARE @Json NVARCHAR(MAX) = N'
								{
									"CategoryCode": "DEFAULT_RESOLUTION"
								}';

EXEC shared.sp_listitem_ddl @Json = @Json;

*/

CREATE OR ALTER PROCEDURE shared.sp_listitem_ddl
(
	@Json AS NVARCHAR(MAX)
)
AS
BEGIN
	
	DECLARE @CategoryCode NVARCHAR(100) = LTRIM(RTRIM(ISNULL(JSON_VALUE(@Json, '$.CategoryCode'), '')));

	SELECT ISNULL(
	(
		SELECT	li.id,
				li.item,
				li.code
		FROM shared.listitem AS li
		INNER JOIN shared.listitem_category AS lic ON li.category_id = lic.id
		WHERE lic.category_code = @CategoryCode 
		FOR JSON PATH, INCLUDE_NULL_VALUES
	), '[]');

END;
