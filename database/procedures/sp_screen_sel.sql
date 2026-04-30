
/*

=====================================================================
- Description: retrieve screen record with its
               operating hours and supported media for grid.
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                 "Filter": {
                                    "TenantId": 1,
                                    "IsActive": 1,
                                    "CountryCodeList": [],
                                    "CityList": [],
                                    "OrientationList": [],
                                    "ResolutionList": []
                                 },
                                 "SearchText": "t",
                                 "Offset": 0,
                                 "PageSize": 10
                                }';

EXEC inv.sp_screen_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE inv.sp_screen_sel
(
    @Json NVARCHAR(MAX)    
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsActive BIT = ISNULL(JSON_VALUE(@Json, '$.Filter.IsActive'), 1),
            @TenantId INT = JSON_VALUE(@Json, '$.Filter.TenantId'),
            @CountryCodeList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.CountryCodeList'), '[]'),
            @CityList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.CityList'), '[]'),
            @OrientationList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.OrientationList'), '[]'),
            @ResolutionList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.ResolutionList'), '[]'),
            @SearchText NVARCHAR(200) = ISNULL(LOWER(JSON_VALUE(@Json, '$.SearchText')), ''),
            @Offset INT = ISNULL(JSON_VALUE(@Json, '$.Offset'), 0),
            @PageSize INT = ISNULL(JSON_VALUE(@Json, '$.PageSize'), 10),
            @TotalRows INT = 0,
            @Query NVARCHAR(MAX);

    CREATE TABLE #screen
    (
        id                  INT NULL,
        uuid                UNIQUEIDENTIFIER NOT NULL,
	    tenant_id			INT NOT NULL,
        tenant_name         NVARCHAR(100) NOT NULL,
	    [name]				NVARCHAR(100) NOT NULL,
	    normalized_name		NVARCHAR(100) NOT NULL,
	    screen_code			NVARCHAR(40) NOT NULL,
	    [description]		NVARCHAR(500) NOT NULL,
	    default_resolution	NVARCHAR(50) NOT NULL,
	    orientation			NVARCHAR(50) NOT NULL,
	    [location]			NVARCHAR(100) NOT NULL,
	    address_line		NVARCHAR(200) NULL,
	    tag					NVARCHAR(50) NOT NULL,
	    country_code		NVARCHAR(50) NOT NULL,
	    city				NVARCHAR(50) NOT NULL,
	    timezone			NVARCHAR(50) NOT NULL,
	    is_active			BIT NOT NULL DEFAULT 1,
	    rate_per_hour		DECIMAL(10,2) NOT NULL,
	    currency			NVARCHAR(50) NOT NULL,
        operating_hour      NVARCHAR(MAX) NULL,
        supported_media     NVARCHAR(MAX) NULL,

        created_by          INT NOT NULL,
        creator             NVARCHAR(100) NOT NULL,
        updated_by          INT NULL,
        modifier            NVARCHAR(100) NOT NULL,
	    created_at          DATETIME2 NOT NULL,
	    updated_at          DATETIME2 NULL
    );

    INSERT INTO #screen
    (
        id,
        uuid,
        tenant_id,
        tenant_name,
        [name],
        normalized_name,
        screen_code,
        [description],
        default_resolution,
        orientation,
        [location],
        address_line,
        tag,
        country_code,
        city,
        timezone,
        is_active,
        rate_per_hour,
        currency,
        operating_hour,
        supported_media,

        created_by,
        creator,
        updated_by,
        modifier,
        created_at,
        updated_at
    )
    SELECT  s.id,
            s.uuid,
            s.tenant_id,
            t.[name]        AS tenant_name,
            s.[name],
            s.normalized_name,
            s.screen_code,
            s.[description],
            s.default_resolution,
            s.orientation,
            s.[location],
            s.address_line,
            JSON_QUERY(s.tag) AS tag,
            s.country_code,
            s.city,
            s.timezone,
            s.is_active,
            s.rate_per_hour,
            s.currency,
            oh.operating_hour,
            sm.supported_media,
            s.created_by,
            CONCAT(cu.[name], ' ', ISNULL(cu.sur_name, '')) AS creator,
            s.updated_by,
            CONCAT(uu.[name], ' ', ISNULL(uu.sur_name, '')) AS modifier,
            s.created_at,
            s.updated_at
    FROM inv.screen AS s
    INNER JOIN [identity].tenant     AS t  ON s.tenant_id   = t.id
    INNER JOIN [identity].[user]     AS cu ON s.created_by  = cu.id
    LEFT  JOIN [identity].[user]     AS uu ON s.updated_by  = uu.id
    LEFT JOIN inv.tf_screen_operating_hour() AS oh ON s.id = oh.screen_id
    LEFT JOIN inv.tf_screen_supported_media() AS sm On s.id = sm.screen_id
    WHERE   
    s.deleted_at IS NULL AND
    s.is_active = @IsActive AND
    s.tenant_id = @TenantId AND
    (
        @SearchText = ''
        OR LOWER(s.screen_code)    LIKE '%' + @SearchText + '%'
        OR s.normalized_name       LIKE '%' + @SearchText + '%'
    ) AND
    (
        @CountryCodeList = '[]'
        OR s.country_code IN (SELECT [value] FROM OPENJSON(@CountryCodeList))
    ) AND
    (
        @CityList = '[]'
        OR s.city IN (SELECT [value] FROM OPENJSON(@CityList))
    ) AND
    (
        @OrientationList = '[]'
        OR s.orientation IN (SELECT [value] FROM OPENJSON(@OrientationList))
    ) AND
    (
        @ResolutionList = '[]'
        OR s.default_resolution IN (SELECT [value] FROM OPENJSON(@ResolutionList))
    );


    SELECT @TotalRows = COUNT(*)
                        FROM #screen;

    CREATE TABLE #result
    (
        [data] NVARCHAR(MAX) NOT NULL
    );

    SET @Query = N'
                SELECT ISNULL(
                        (
                            SELECT  ts.id,
                                    ts.uuid,
                                    ts.tenant_id,
                                    ts.[name]        AS tenant_name,
                                    ts.[name],
                                    ts.normalized_name,
                                    ts.screen_code,
                                    ts.[description],
                                    ts.default_resolution,
                                    ts.orientation,
                                    ts.[location],
                                    ts.address_line,
                                    JSON_QUERY(ts.tag) AS tag,
                                    ts.country_code,
                                    ts.city,
                                    ts.timezone,
                                    ts.is_active,
                                    ts.rate_per_hour,
                                    ts.currency,
                                    JSON_QUERY(ts.operating_hour) AS operating_hour,
                                    JSON_QUERY(ts.supported_media) AS supported_media,
                                    ts.created_by,
                                    ts.creator,
                                    ts.updated_by,
                                    ts.modifier,
                                    ts.created_at,
                                    ts.updated_at 
                            FROM #screen AS ts 
                            ORDER BY ts.normalized_name
                            OFFSET ' + CAST(@Offset AS VARCHAR(20)) + N' ROWS
                            FETCH NEXT ' + CAST(@PageSize AS VARCHAR(20)) + N' ROWS ONLY 
                            FOR JSON PATH, INCLUDE_NULL_VALUES
                        )
                       , ''[]'')';

    INSERT INTO #result
    (
        [data]
    )
    EXEC ( @Query ); 

    SELECT ISNULL((
			SELECT 
                JSON_QUERY([data]) AS [data],
				@TotalRows AS [total_rows]
			FROM #result
			FOR JSON PATH,
				INCLUDE_NULL_VALUES,
				WITHOUT_ARRAY_WRAPPER
			), '[]');

    DROP TABLE IF EXISTS #screen, #result;
END