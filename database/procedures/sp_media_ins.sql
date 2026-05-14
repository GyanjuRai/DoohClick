

/*

=====================================================================
- Description: Insert media.
- Author: Gyanju Rai
- Created: 2026-05-04
=====================================================================

DECLARE @Json NVARCHAR(MAX) = '{
                                    "TenantId": 1,
                                    "DisplayName": "McDonalds Times Square 30s",
                                    "AdvertiserId": 1,
                                    "FileName": "a3f7c821-9b2e-4d1a-bc34-7e6f09d12345.mp4",
                                    "FileUrl": "/media/videos/a3f7c821-9b2e-4d1a-bc34-7e6f09d12345.mp4",
                                    "FileSizeBytes": 4,
                                    "Resolution": "1920x1080",
                                    "DurationSec": 30,
                                    "IsVideo": true,
                                    "UploadedBy": 1,
                                    "CreatedBy": 1
                                }';

EXEC dbo.sp_media_ins  @Json = @Json OUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_media_ins 
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
    
    CREATE TABLE #inserted
    (
        id INT NOT NULL
    );

    CREATE TABLE #media
    (
	    tenant_id		INT NOT NULL,
	    display_name	NVARCHAR(100) NOT NULL,
        advertiser_id    INT NULL,
	    [file_name]		NVARCHAR(255) NULL,
	    file_url		NVARCHAR(500) NULL,
        file_size_bytes BIGINT NULL,
	    resolution		NVARCHAR(50)  NULL,
	    duration_sec	DECIMAL(10,3) NULL,
	    [status]		NVARCHAR(50) NOT NULL,
	    is_video		BIT NULL,
	    is_deleted		BIT NOT NULL DEFAULT 0,
	    uploaded_by		INT NULL,
	    uploaded_at		DATETIME2 NULL,
	    created_by		INT NOT NULL,
	    created_at		DATETIME2 NOT NULL
    );

    INSERT INTO #media
    (
        tenant_id,
        display_name,
        advertiser_id,
        [file_name],
        file_url,
        file_size_bytes,
        resolution,
        duration_sec,
        [status],
        is_video,
        uploaded_by,
        uploaded_at,
        created_by,
        created_at
    )
    SELECT  oj.TenantId,
            oj.DisplayName,
            oj.AdvertiserId,
            oj.[FileName],
            oj.FileUrl,
            oj.FileSizeBytes,
            oj.Resolution,
            oj.DurationSec,
            'READY',
            oj.IsVideo,
            oj.UploadedBy,
            GETUTCDATE(),
            oj.CreatedBy,
            GETUTCDATE()
    FROM OPENJSON(@Json)
    WITH
    (
        TenantId INT,
        DisplayName NVARCHAR(100),
        AdvertiserId INT,
        [FileName] NVARCHAR(255),
        FileUrl NVARCHAR(500),
        FileSizeBytes BIGINT,
        Resolution NVARCHAR(50),
        DurationSec DECIMAL(10,3),
        IsVideo BIT,
        UploadedBy INT,
        CreatedBy INT
    ) AS oj;

    BEGIN TRANSACTION

    INSERT INTO dbo.media_library
    (
        tenant_id,
        display_name,
        advertiser_id,
        [file_name],
        file_url,
        file_size_bytes,
        resolution,
        [status],
        duration_sec,
        is_video,
        uploaded_by,
        uploaded_at,
        created_by,
        created_at
    )
    OUTPUT Inserted.id INTO #inserted ( id )
    SELECT  tm.tenant_id,
            tm.display_name,
            tm.advertiser_id,
            tm.[file_name],
            tm.file_url,
            tm.file_size_bytes,
            tm.resolution,
            tm.[status],
            tm.duration_sec,
            tm.is_video,
            tm.uploaded_by,
            tm.uploaded_at,
            tm.created_by,
            tm.created_at
    FROM #media AS tm;

    COMMIT TRANSACTION;

    SELECT @Json = ISNULL((
            SELECT  ml.id,
                    ml.tenant_id,
                    ml.display_name,
                    ml.advertiser_id,
                    ml.[file_name],
                    ml.file_url,
                    ml.file_size_bytes,
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
            WHERE EXISTS (
                SELECT 1
                FROM #inserted WHERE id = ml.id
            )
            FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
    ), '{}');

    DROP TABLE #inserted, #media;

    END TRY
    BEGIN CATCH
        
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DROP TABLE IF EXISTS #inserted, #media;
        THROW;
        
    END CATCH
END