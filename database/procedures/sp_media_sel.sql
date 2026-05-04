

/*

=====================================================================
- Description: retrieve media record for grid.
- Author: Gyanju Rai
- Created: 2026-05-04
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                 "Filter": {
                                    "TenantId": 1,
                                    "StatusList": ["PENDING", "READY"],
                                    "IsVideo": 1
                                 },
                                 "SearchText": "",
                                 "Offset": 0,
                                 "PageSize": 10
                                }';

EXEC dbo.sp_media_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_media_sel
(
    @Json NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TenantId INT = ISNULL(JSON_VALUE(@Json, '$.Filter.TenantId'), 0),
            @StatusList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.StatusList'), '[]'),
            @IsVideo BIT = JSON_VALUE(@Json, '$.Filter.IsVideo'),
            @SearchText NVARCHAR(200) = ISNULL(LOWER(JSON_VALUE(@Json, '$.SearchText')), ''),
            @Offset INT = ISNULL(JSON_VALUE(@Json, '$.Offset'), 0),
            @PageSize INT = ISNULL(JSON_VALUE(@Json, '$.PageSize'), 10),
            @TotalRows INT = 0,
            @Query NVARCHAR(MAX);

    CREATE TABLE #media
    (
        id              INT NOT NULL,
        tenant_id		INT NOT NULL,
	    display_name	NVARCHAR(100) NOT NULL,
	    [file_name]		NVARCHAR(255) NULL,
	    file_url		NVARCHAR(500) NULL,
        file_size_bytes BIGINT NULL,
	    resolution		NVARCHAR(50)  NULL,
	    [status]		NVARCHAR(50) NOT NULL,
	    duration_sec	DECIMAL(10,3) NULL,
	    is_video		BIT NULL,
	    uploaded_by		INT NULL,
	    uploaded_at		DATETIME2 NULL,
        uploader        NVARCHAR(100) NULL,
	    created_by		INT NOT NULL,
	    created_at		DATETIME2 NOT NULL,
        creator         NVARCHAR(100) NULL
    );

    INSERT INTO #media
    (
        id,
        tenant_id,
        display_name,
        [file_name],
        file_url,
        file_size_bytes,
        resolution,
        [status],
        duration_sec,
        is_video,
        uploaded_by,
        uploaded_at,
        uploader,
        created_by,
        created_at,
        creator
    )
    SELECT  ml.id,
            ml.tenant_id,
            ml.display_name,
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
    WHERE   ml.tenant_id = @TenantId AND 
            ml.is_deleted = 0 AND 
            (@SearchText = '' OR LOWER(ml.display_name) LIKE N'%' + @SearchText + N'%') AND 
		    (
			    @IsVideo IS NULL OR ml.is_deleted = @IsVideo
		    ) AND
            (
                @StatusList = '[]' OR ml.[status] IN (
                    SELECT [value]
                    FROM OPENJSON(@StatusList)
                )
            );

    SELECT @TotalRows = COUNT(*) FROM #media;

    SET @Query = N'
		SELECT ISNULL(
				(
					SELECT	*
					FROM #media
					ORDER BY [display_name]
					OFFSET ' + CAST(@Offset AS VARCHAR(20)) + N' ROWS
					FETCH NEXT ' + CAST(@PageSize AS VARCHAR(20)) + N' ROWS ONLY 
					FOR JSON PATH, INCLUDE_NULL_VALUES
				), ''[]'')';

	CREATE TABLE #result
	(
		[data] NVARCHAR(MAX)
	);

	INSERT INTO #result
	(
		[data]
	)
	EXEC (@Query)
	
	SELECT ISNULL((
			SELECT 
                JSON_QUERY([data]) AS [data],
				@TotalRows AS [total_rows]
			FROM #result
			FOR JSON PATH,
				INCLUDE_NULL_VALUES,
				WITHOUT_ARRAY_WRAPPER
			), '[]');

	DROP TABLE IF EXISTS #media;
	DROP TABLE IF EXISTS #result;
    
END;