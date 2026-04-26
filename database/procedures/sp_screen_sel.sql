
/*

=====================================================================
- Description: retrieve screen record with its
               operating hours and supported media for gridd.
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                 "Filter": {
                                    "IsActive": 1,
                                    "CountryCodeList": [],
                                    "CityList": [],
                                    "OrientationList": [],
                                    "DefaultResolution": []
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

    DECLARE @IsActive BIT = ISNULL(JSON_VALUE(@Json, '$.Filter.IsActive'), '[]'),
            @CountryCodeList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.CountryCodeList'), '[]'),
            @CityList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.CityList'), '[]'),
            @OrientationList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.OrientationList'), '[]'),
            @DefaultResolution NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.OrientationList'), '[]'),
            @SearchText NVARCHAR(200) = ISNULL(LOWER(JSON_VALUE(@Json, '$.SearchText')), ''),
            @Offset INT = ISNULL(JSON_VALUE(@Json, '$.Offset'), 0),
            @PageSize INT = ISNULL(JSON_VALUE(@Json, '$.PageSize'), 10),
            @TotalRows INT = 0,
            @Query NVARCHAR(MAX);

    CREATE TABLE #screen
    (
        id                  INT NULL,
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
            JSON_QUERY((
                SELECT  oh.id,
                        oh.day_of_week,
                        oh.open_time,
                        oh.close_time,
                        oh.audience_source,
                        oh.estimated_impression
                FROM inv.screen_operating_hour AS oh
                WHERE oh.screen_id = s.id
                FOR JSON PATH, INCLUDE_NULL_VALUES
            )) AS operating_hour,
            JSON_QUERY((
                SELECT  sm.id,
                        sm.media_type
                FROM inv.screen_supported_media AS sm
                WHERE sm.screen_id = s.id
                FOR JSON PATH, INCLUDE_NULL_VALUES
            )) AS supported_media,
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
    WHERE   
    s.deleted_at IS NULL AND
    s.is_active = @IsActive AND
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
        @DefaultResolution = '[]'
        OR s.default_resolution IN (SELECT [value] FROM OPENJSON(@DefaultResolution))
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
                            SELECT * 
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