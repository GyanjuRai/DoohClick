
/*

=====================================================================
- Description: retrieve active and not deleted advertiser record for 
				dropdown.
- Author: Gyanju Rai
- Created: 2026-05-04
=====================================================================

DECLARE @Json NVARCHAR(100) = N'
								{
									"TenantId": 2
								}';

EXEC crm.sp_advertiser_ddl @Json = @Json;

*/

CREATE OR ALTER PROCEDURE crm.sp_advertiser_ddl
(
	@Json NVARCHAR(100)
)
AS
BEGIN
	
	DECLARE @TenantId INT = ISNULL(JSON_VALUE(@Json, '$.TenantId'), 0);

	SELECT ISNULL((
		SELECT	id,
				[name]
		FROM crm.advertiser
		WHERE is_active = 1 AND is_deleted = 0 AND tenant_id = @TenantId
		FOR JSON PATH, INCLUDE_NULL_VALUES
	), '[]');

END;