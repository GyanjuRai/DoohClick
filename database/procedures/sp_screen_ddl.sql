
/*

============================================
- Description: get dropdown list of screen.
- Author: Gyanju Rai
- Created: 2026-05-07
============================================

DECLARE @Json NVARCHAR(100) = N'{ 
                                    "TenantId": 1 
                                }';

EXEC inv.sp_screen_ddl @Json = @Json;

*/

CREATE OR ALTER PROCEDURE inv.sp_screen_ddl
(
    @Json NVARCHAR(100) 
)
AS
BEGIN
    
    DECLARE @TenantId INT = JSON_VALUE(@Json, '$.TenantId');

    SELECT ISNULL((
        SELECT  s.id,
                s.[name],
                s.normalized_name,
                s.screen_code,
                s.default_resolution,
                s.orientation,
                JSON_QUERY(s.tag) AS tag,
                s.country_code,
                s.city,
                s.timezone,
                oh.operating_hour,
                sm.supported_media
        FROM inv.screen AS s
        LEFT JOIN inv.tf_screen_operating_hour() AS oh ON s.id = oh.screen_id
        LEFT JOIN inv.tf_screen_supported_media() AS sm On s.id = sm.screen_id
         WHERE   
        s.deleted_at IS NULL AND
        s.is_active = 1 AND
        s.tenant_id = @TenantId
        FOR JSON PATH, INCLUDE_NULL_VALUES
    ), '[]');
    
END