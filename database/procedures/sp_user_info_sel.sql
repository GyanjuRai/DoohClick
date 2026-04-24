
/*

=====================================================================
- Description: Retrieve user information uuid, role, fullname, email.
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(200) = N'
								{
									"UserUuid": "67B7FC49-D3A9-4F10-85ED-F1726BCA7D56",
									"TenantCode": "lamar"
								}';

EXEC [identity].sp_user_info_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE [identity].sp_user_info_sel
(
	@Json NVARCHAR(500)
)
AS
BEGIN
	
	DECLARE @UserUuid UNIQUEIDENTIFIER = TRIM(JSON_VALUE(@Json, '$.UserUuid')),
			@TenantCode NVARCHAR(40) = UPPER(ISNULL(LTRIM(RTRIM(JSON_VALUE(@Json, '$.TenantCode'))), ''));

	SELECT ISNULL((
			SELECT 
				u.id AS [user_id],
				u.uuid AS [user_uuid],
				t.id AS [tenant_id],
				t.tenant_code,
				CONCAT (
					u.[name],
					' ',
					ISNULL(u.sur_name, '')
					) AS full_name,
				u.user_role,
				u.email
			FROM [identity].[user] AS u
			INNER JOIN [identity].tenant AS t ON u.tenant_id = t.id
			WHERE u.uuid = @UserUuid AND t.tenant_code = @TenantCode AND u.is_deleted != 1
			FOR JSON PATH,
				WITHOUT_ARRAY_WRAPPER
			), '{}');

END;