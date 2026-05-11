
/*

=========================================================================================
- Description: grid select campaign details with list of campaign flight (campaign date)
                and campaign flight screen (campaign screen).
- Author: Gyanju Rai
- Created: 2026-05-07
==========================================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                    "Filter": {
										"TenantId": 1,
										"Status": "DRAFT",
										"AdvertiserIdList": [],
										"StartDate": "",
										"EndDate": ""
                                    },
                                    "SearchText": "",
                                    "Offset": 0,
                                    "PageSize": 10
                                }';

EXEC dbo.sp_campaign_sel @Json = @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_campaign_sel
(
    @Json NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON

	DECLARE @TenantId INT = ISNULL(JSON_VALUE(@Json, '$.Filter.TenantId'), 0),
			@Status NVARCHAR(50) = ISNULL(JSON_VALUE(@Json, '$.Filter.Status'), 'DRAFT'),
			@AdvertiserIdList NVARCHAR(MAX) = ISNULL(JSON_QUERY(@Json, '$.Filter.AdvertiserIdList'), '[]'),
			@StartDate DATE = TRY_CAST(JSON_VALUE(@Json, '$.Filter.StartDate') AS DATE),
			@EndDate DATE = TRY_CAST(JSON_VALUE(@Json, '$.EndDate') AS DATE),
			@SearchText NVARCHAR(255) = ISNULL(JSON_VALUE(@Json, '$.SearchText'), ''),
			@Offset INT = ISNULL(JSON_VALUE(@Json, '$.Offset'), 0),
			@PageSize INT = ISNULL(JSON_VALUE(@Json, '$.PageSize'), 10),
			@Query NVARCHAR(MAX),
			@TotalRows INT;

    CREATE TABLE #campaign
    (
        id INT NULL,
		campaign_code NVARCHAR(50) NULL,
		tenant_id INT NULL,
		advertiser_id INT NULL,
		advertiser NVARCHAR(100) NULL,
		[name] NVARCHAR(150) NULL,
		[status] NVARCHAR(50) NULL,
		[start_date] DATE NULL,
		[end_date] DATE NULL,
		duration_in_days INT NULL,
		remarks NVARCHAR(255) NULL,
		is_locked BIT NULL DEFAULT(0),
		created_by INT NULL,
		created_at DATETIME2 NULL DEFAULT(GETUTCDATE()),
		creator NVARCHAR(100) NULL,
		modified_by INT NULL,
		modified_at DATETIME2 NULL,
		modifier NVARCHAR(100) NULL,
		campaign_flight NVARCHAR(MAX) NULL
    );

	INSERT INTO #campaign
	(
		id,
		campaign_code,
		tenant_id,
		advertiser_id,
		advertiser,
		[name],
		[status],
		[start_date],
		end_date,
		duration_in_days,
		remarks,
		is_locked,
		created_by,
		created_at,
		creator,
		modified_by,
		modified_at,
		modifier,
		campaign_flight
	)
	SELECT	c.id,
			c.campaign_code,
			c.tenant_id,
			c.advertiser_id,
			a.[name],
			c.[name],
			c.[status],
			c.[start_date],
			c.end_date,
			c.duration_in_days,
			c.remarks,
			c.is_locked,
			c.created_by,
			c.created_at,
			( RTRIM (
				LTRIM (
				CONCAT (
					COALESCE (cu.[name] + ' ', ''),
					COALESCE (cu.sur_name, '')
				)
			))) AS creator,
			c.modified_by,
			c.modified_at,
			( RTRIM (
				LTRIM (
				CONCAT (
					COALESCE (mu.[name] + ' ', ''),
					COALESCE (mu.sur_name, '')
				)
			))) AS modifier,
			ISNULL(cfs.campaign_flight, '[]') AS campaign_flight
	FROM dbo.campaign AS c
	LEFT JOIN dbo.tf_campaign_flight_screen() AS cfs ON c.id = cfs.campaign_id
	INNER JOIN crm.advertiser AS a ON c.advertiser_id = a.id
	INNER JOIN [identity].[user] AS cu ON c.created_by = cu.id
	LEFT JOIN [identity].[user] AS mu ON c.modified_by = mu.id
	WHERE	
	c.tenant_id = @TenantId AND
	c.is_deleted = 0		AND
	c.[status] = @Status	AND
	(
		@AdvertiserIdList = '[]' OR
		c.advertiser_id IN (
							SELECT [value]
							FROM OPENJSON(@AdvertiserIdList)
						)
	)						AND
	(
		@StartDate IS NULL OR c.end_date   >= @StartDate
	)						AND
	(
		@EndDate IS NULL OR c.[start_date] <= @EndDate
	);

	SELECT @TotalRows = COUNT(1) FROM
						#campaign;

	SET @Query = N'
					SELECT ISNULL(
								(
									SELECT	tc.id,
											tc.campaign_code,
											tc.tenant_id,
											tc.advertiser_id,
											tc.advertiser,
											tc.[name],
											tc.[status],
											tc.start_date,
											tc.end_date,
											tc.duration_in_days,
											tc.remarks,
											tc.is_locked,
											tc.created_by,
											tc.created_at,
											tc.creator,
											tc.modified_by,
											tc.modified_at,
											tc.modifier,
											JSON_QUERY(tc.campaign_flight)        AS campaign_flight
									FROM #campaign AS tc
									ORDER BY tc.[name]
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

	DROP TABLE IF EXISTS #campaign, #result;
END