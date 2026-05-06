
/*

=====================================================================
- Description: soft delete media.
- Author: Gyanju Rai
- Created: 2026-05-04
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                    "TenantId": 1,
                                    "Id": 1,
                                    "DeletedBy": 1
                                 }';

EXEC dbo.sp_media_del @Json = @Json OUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_media_del
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        DECLARE @TenantId INT = ISNULL(JSON_VALUE(@Json, '$.TenantId'), 0),
                @Id INT = ISNULL(JSON_VALUE(@Json, '$.Id'), 0),
                @DeletedBy INT = JSON_VALUE(@Json, '$.DeletedBy');
        
        BEGIN TRANSACTION
        UPDATE m
        SET m.is_deleted = 1,
            m.deleted_by = 1,
            m.deleted_at = GETUTCDATE(),
            m.[status] = 'ARCHIVED'
        FROM dbo.media_library AS m
        WHERE m.id = @Id AND m.tenant_id = @TenantId;

        COMMIT TRANSACTION

        SELECT @Json = ISNULL(
            (
                SELECT  ml.id,
                        ml.tenant_id,
                        ml.display_name,
                        ml.[file_name],
                        ml.file_url,
                        ml.resolution,
                        ml.[status],
                        ml.duration_sec,
                        ml.is_video,
                        ml.uploaded_by,
                        ml.uploaded_at,
                        [identity].sf_get_user_fullname_by_id(ml.uploaded_by) AS uploader,
                        ml.created_by,
                        [identity].sf_get_user_fullname_by_id(ml.created_by) AS creator,
                        ml.created_at
                FROM dbo.media_library AS ml
                WHERE ml.id = @Id AND ml.tenant_id = @TenantId
                FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
        )
        , '{}');
    
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION

        THROW
    END CATCH;
END