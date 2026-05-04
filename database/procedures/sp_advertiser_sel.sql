
/*

=====================================================================
- Description: retrieve advertiser record for grid.
- Author: Gyanju Rai
- Created: 2026-05-04
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                 "Filter": {
                                    "TenantId": 1,
                                    "IsActiveList": [1, 0]
                                 },
                                 "SearchText": "",
                                 "Offset": 0,
                                 "PageSize": 10
                                }';

EXEC crm.sp_advertiser_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE crm.sp_advertiser_sel
(
    @Json NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsActiveList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.IsActiveList'), '[]'),
            @TenantId INT = JSON_VALUE(@Json, '$.Filter.TenantId'),
            @SearchText NVARCHAR(200) = ISNULL(LOWER(JSON_VALUE(@Json, '$.SearchText')), ''),
            @Offset INT = ISNULL(JSON_VALUE(@Json, '$.Offset'), 0),
            @PageSize INT = ISNULL(JSON_VALUE(@Json, '$.PageSize'), 10),
            @TotalRows INT = 0,
            @Query NVARCHAR(MAX);

    CREATE TABLE #advertiser
    (
        id				INT NULL,
	    uuid			UNIQUEIDENTIFIER NULL,
	    tenant_id		INT NOT NULL,
	    [name]			NVARCHAR(100) NOT NULL,
	    contact_name	NVARCHAR(60) NOT NULL,
	    contact_email	NVARCHAR(60) NOT NULL,
	    contact_phone	NVARCHAR(20) NOT NULL,
	    is_active		BIT NOT NULL,

	    created_by		INT NOT NULL,
		creator			NVARCHAR(100) NOT NULL,
	    created_at		DATETIME2 NOT NULL,
	    updated_by		INT  NULL,
	    updated_at		DATETIME2 NULL
    )

    INSERT INTO #advertiser 
	(
		id,
		uuid,
		tenant_id,
		[name],
		contact_name,
		contact_email,
		contact_phone,
		is_active,
		created_by,
		creator,
		created_at,
		updated_by,
		updated_at
	)
	SELECT	a.id,
			a.uuid,
			a.tenant_id,
			a.[name],
			a.contact_name,
			a.contact_email,
			a.contact_phone,
			a.is_active,
			a.created_by,
			CONCAT(u.[name], ' ', ISNULL(u.sur_name, '')) AS creator,
			a.created_at,
			a.updated_by,
			a.updated_at
	FROM crm.advertiser AS a
	INNER JOIN [identity].[user] AS u ON a.created_by = u.id
	WHERE 
		a.is_deleted = 0 AND 
		a.tenant_id = @TenantId AND 
		(@SearchText = '' OR LOWER(a.[name]) LIKE N'%' + @SearchText + N'%') AND 
		(
			@IsActiveList = '[]' OR a.is_active IN (
				SELECT [value]
				FROM OPENJSON(@IsActiveList)
				)
		);

	SELECT @TotalRows = COUNT(*) FROM #advertiser;

	SET @Query = N'
		SELECT ISNULL(
				(
					SELECT	*
					FROM #advertiser
					ORDER BY [name]
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

	DROP TABLE IF EXISTS #advertiser;
	DROP TABLE IF EXISTS #result;
END;