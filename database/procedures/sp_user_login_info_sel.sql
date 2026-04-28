

/*

=====================================================================
- Description: Retrieve user login information username, passwordhash
				and id
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(200) = N'
								{
									"UserName": "lamar.admin",
									"TenantCode": "lamar"
								}';

EXEC [identity].sp_user_login_info_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE [identity].sp_user_login_info_sel
(
	@Json NVARCHAR(200)
)
AS
BEGIN
	
	DECLARE @UserName NVARCHAR(100) = ISNULL(LTRIM(RTRIM(JSON_VALUE(@Json, '$.UserName'))), ''),
			@TenantCode NVARCHAR(40) = UPPER(ISNULL(LTRIM(RTRIM(JSON_VALUE(@Json, '$.TenantCode'))), '')),
			@NormalizedUserName NVARCHAR(100);

	SET @NormalizedUserName = LOWER(@UserName);

	SELECT ISNULL((
				SELECT	u.uuid AS [user_uuid],
						u.password_hash,
						t.tenant_code
				FROM [identity].[user] AS u
				INNER JOIN [identity].tenant AS t ON u.tenant_id = t.id
				WHERE u.[user_name] = @NormalizedUserName AND u.is_active != 0 AND u.is_deleted != 1 AND t.tenant_code = @TenantCode
				FOR JSON PATH,
					WITHOUT_ARRAY_WRAPPER
				), '{}');
END