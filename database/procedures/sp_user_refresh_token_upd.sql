
/*

=====================================================================
- Description: Retrieve user login information username, passwordhash
				and id
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(200) = N'
								{
									"UserId": 1,
									"RefreshToken": "",
									"RefreshTokenExpiry": "",
								}';

EXEC [identity].sp_user_refresh_token_upd @Json = @Json OUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE [identity].sp_user_refresh_token_upd
(
	@Json NVARCHAR(MAX) OUT
)
AS
BEGIN
	
	DECLARE @UserId INT = JSON_VALUE(@Json, '$.UserId'),
			@RefreshToken NVARCHAR(500) = JSON_VALUE(@Json, '$.RefreshToken'),
			@RefreshTokenExpiry DATETIME2 = JSON_VALUE(@Json, '$.RefreshTokenExpiry');

	UPDATE u
	SET	u.refresh_token = @RefreshToken,
		u.refresh_token_expiry = @RefreshTokenExpiry
	FROM [identity].[user] AS u
	WHERE u.id = @UserId;

	SELECT @Json = ISNULL((
			SELECT u.refresh_token,
				u.refresh_token_expiry
			FROM [identity].[user] AS u
			WHERE u.id = @UserId
			FOR JSON PATH,
				INCLUDE_NULL_VALUES,
				WITHOUT_ARRAY_WRAPPER
			), '{}');

END