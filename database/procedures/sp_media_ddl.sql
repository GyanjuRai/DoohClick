

/*

=====================================================================
- Description: retrieve media for ddl.
- Author: Gyanju Rai
- Created: 2026-05-04
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                    "TenantId": 1
                                 }';

EXEC dbo.sp_media_ddl @Json = @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_media_ddl
(
    @Json NVARCHAR(MAX)
)
AS
BEGIN
    DECLARE @TenantId INT = ISNULL(JSON_VALUE(@Json, '$.TenantId'), 0);

    SELECT ISNULL(
        (
            SELECT  ml.id,
                    ml.display_name,
                    ml.file_url,
                    ml.file_size_bytes
            FROM dbo.media_library AS ml
            WHERE   ml.tenant_id = @TenantId AND
                    ml.[status] = 'READY' AND
                    ml.is_deleted = 0
            FOR JSON PATH
        )
    , '[]');
END