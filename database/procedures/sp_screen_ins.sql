
/*

=====================================================================
- Description: Insert screen.
- Author: Gyanju Rai
- Created: 2026-04-05
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'
								{
								  "Id": null,
								  "TenantId": "A9C499BA-F4F3-4A00-9BD7-8E38E53FBBE2",
								  "Name": "Times Square Billboard",
								  "NormalizedName": "TIMES SQUARE BILLBOARD",
								  "Status": 1,
								  "Tier": 2,
								  "Category": 1,
								  "Subcategory": 3,
								  "OrientationType": 1,
								  "Width": 1920,
								  "Height": 1080,
								  "SupportedFormats": "MP4,JPG,PNG",
								  "MaxFilesizeMb": 50,
								  "MaxDurationSeconds": 30,
								  "SupportsAudio": true,
								  "SupportsHtml5": true,
								  "Country": 1,
								  "City": 5,
								  "District": 12,
								  "VenueType": 2,
								  "VenueName": "Times Square",
								  "Latitude": 40.757990,
								  "Longitude": -73.985580,
								  "TimeZone": "America/New_York",
								  "CurrencyCode": 1,
								  "Amount": 1500.0000,
								  "PlayerId": "7C9E6679-7425-40DE-944B-E07FC1F90AE7",
								  "RulesetId": "8D9E6679-7425-40DE-944B-E07FC1F90AE8",
								  "ConcurrencyStamp": "abc123xyz456def789ghi012jkl345mn",
								  "CreatedBy": "6557E3D1-5DC3-4A3C-A07C-FB2594C031B2",
								  "CreatedAt": "2025-01-15T10:30:00"
								}';

EXEC inv.sp_screen_sel @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE inv.sp_screen_ins
(
	@Json AS NVARCHAR(MAX) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @ScreenStatus INT = 14; -- sf_get_listItem_id

		CREATE TABLE #screen
		(
			id UNIQUEIDENTIFIER NULL,
			tenant_id UNIQUEIDENTIFIER NOT NULL,
			[name] NVARCHAR(50) NOT NULL,
			normalized_name NVARCHAR(50) NOT NULL,
			[status] INT NOT NULL,
			tier INT NOT NULL,
			category INT NOT NULL,
			subcategory INT NULL,
			orientation_type INT NOT NULL,
			width INT NULL,
			height INT NULL,
			supported_formats NVARCHAR(MAX) NULL,
			max_filesize_mb INT NULL,
			max_duration_seconds INT NULL,
			supports_audio BIT NULL,
			supports_html5 BIT NULL,
			country INT NULL,
			city INT NULL,
			district INT NULL,
			venue_type INT NULL,
			venue_name NVARCHAR(100) NULL,
			latitude DECIMAL(9,6) NULL,
			longitude DECIMAL(10, 6) NULL,
			time_zone VARCHAR(50) NOT NULL,
			currency_code INT NOT NULL,
			amount DECIMAL(19,4) NULL,
			player_id UNIQUEIDENTIFIER NULL,
			ruleset_id UNIQUEIDENTIFIER NULL,
			concurrency_stamp NVARCHAR(40) NULL,
			created_by UNIQUEIDENTIFIER NOT NULL
		);

		INSERT INTO #screen
		(
			id,
			tenant_id,
			[name],
			normalized_name,
			[status],
			tier,
			category,
			subcategory,
			orientation_type,
			width,
			height,
			supported_formats,
			max_filesize_mb,
			max_duration_seconds,
			supports_audio,
			supports_html5,
			country,
			city,
			district,
			venue_type,
			venue_name,
			latitude,
			longitude,
			time_zone,
			currency_code,
			amount,
			player_id,
			ruleset_id,
			concurrency_stamp,
			created_by
		)
		SELECT	NEWID(),
				oj.TenantId,
				oj.[Name],
				oj.NormalizedName,
				ISNULL(oj.[Status], @ScreenStatus),
				oj.Tier,
				oj.Category,
				oj.Subcategory,
				oj.OrientationType,
				oj.Width,
				oj.Height,
				ISNULL(oj.SupportedFormats, '[]'),
				oj.MaxFilesizeMb,
				oj.MaxDurationSeconds,
				oj.SupportsAudio,
				oj.SupportsHtml5,
				oj.Country,
				oj.City,
				oj.District,
				oj.VenueType,
				oj.VenueName,
				oj.Latitude,
				oj.Longitude,
				oj.TimeZone,
				oj.CurrencyCode,
				oj.Amount,
				oj.PlayerId,
				oj.RulesetId,
				CONVERT(NVARCHAR(40), NEWID()),
				oj.CreatedBy 
		FROM OPENJSON(@Json)
		WITH
		(
			Id UNIQUEIDENTIFIER,
			TenantId UNIQUEIDENTIFIER,
			[Name] NVARCHAR(50),
			NormalizedName NVARCHAR(50),
			[Status] INT,
			Tier INT,
			Category INT,
			Subcategory INT,
			OrientationType INT,
			Width INT,
			Height INT,
			SupportedFormats NVARCHAR(MAX),
			MaxFilesizeMb INT,
			MaxDurationSeconds INT,
			SupportsAudio BIT,
			SupportsHtml5 BIT,
			Country INT,
			City INT,
			District INT,
			VenueType INT,
			VenueName NVARCHAR(100),
			Latitude DECIMAL(9,6),
			Longitude DECIMAL(10,6),
			TimeZone VARCHAR(10),
			CurrencyCode INT,
			Amount DECIMAL(19,4),
			PlayerId UNIQUEIDENTIFIER,
			RulesetId UNIQUEIDENTIFIER,
			ConcurrencyStamp NVARCHAR(40),
			CreatedBy UNIQUEIDENTIFIER
		) AS oj;

		INSERT INTO inv.screen
		(
			id,
			tenant_id,
			[name],
			normalized_name,
			[status],
			tier,
			category,
			subcategory,
			orientation_type,
			width,
			height,
			supported_formats,
			max_filesize_mb,
			max_duration_seconds,
			supports_audio,
			supports_html5,
			country,
			city,
			district,
			venue_type,
			venue_name,
			latitude,
			longitude,
			time_zone,
			currency_code,
			amount,
			player_id,
			ruleset_id,
			concurrency_stamp,
			created_by,
			created_at,
			is_deleted
		)
		SELECT	ts.id,
				ts.tenant_id,
				ts.[name],
				ts.normalized_name,
				ts.[status],
				ts.tier,
				ts.category,
				ts.subcategory,
				ts.orientation_type,
				ts.width,
				ts.height,
				ts.supported_formats,
				ts.max_filesize_mb,
				ts.max_duration_seconds,
				ts.supports_audio,
				ts.supports_html5,
				ts.country,
				ts.city,
				ts.district,
				ts.venue_type,
				ts.venue_name,
				ts.latitude,
				ts.longitude,
				ts.time_zone,
				ts.currency_code,
				ts.amount,
				ts.player_id,
				ts.ruleset_id,
				ts.concurrency_stamp,
				ts.created_by,
				GETUTCDATE(),
				0
		FROM #screen AS ts
		WHERE ts.id IS NULL;

		SELECT @Json = ISNULL(
								(
								SELECT	s.id,
										s.[name],
										s.normalized_name,
										s.[status],
										s.tier,
										JSON_QUERY((SELECT
														s.category,
														s.subcategory,
														s.orientation_type
														FOR JSON PATH, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES
													)) AS [type],
										JSON_QUERY((SELECT
														s.width,
														s.height,
														s.supported_formats,
														s.max_filesize_mb,
														s.max_duration_seconds,
														s.supports_audio,
														s.supports_html5
														FOR JSON PATH, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES
												)) AS capability,
			
										JSON_QUERY((SELECT	
															s.country AS [country_id],
															lic.item AS [country],
															s.city AS [city_id],
															lict.item AS [city],
															s.venue_type,
															s.venue_name,
															s.latitude,
															s.longitude,
															s.time_zone
															FOR JSON PATH, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES
													)) AS [location],
			
										JSON_QUERY((SELECT
														s.currency_code, -- del
														s.amount
														FOR JSON PATH, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES
													)) AS base_price,

										JSON_QUERY((SELECT
															s.player_id,
															ISNULL(p.[name], ''),
															p.[status],
															p.device_type
															FOR JSON PATH, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES
													)) AS player,
			
										JSON_QUERY((SELECT
															s.ruleset_id,
															ISNULL(rs.[name], ''),
															ISNULL(rs.blocked_categories, '[]'),
															rs.max_slots_per_loop,
															rs.min_time_between_same_ad
															FOR JSON PATH, WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES
													)) AS ruleset,

										s.concurrency_stamp,
										s.created_at,
										s.created_by,
										s.updated_at,
										s.updated_by,
										s.approved_at, --del
										s.approved_by --del
								FROM inv.screen AS s
								INNER JOIN #screen AS ts ON s.id = ts.id
								LEFT JOIN shared.listitem AS lic ON s.country = lic.id
								LEFT JOIN shared.listitem AS lict ON s.city = lict.id
								LEFT JOIN shared.listitem AS lid ON s.district = lid.id
								LEFT JOIN inv.player AS p ON s.player_id = p.id
								OUTER APPLY (
									SELECT TOP 1.*
									FROM inv.ruleset
									WHERE (
											(s.ruleset_id = id OR is_default = 1) 
											AND is_deleted != 0
											AND [status] = 18
											)
									ORDER BY CASE WHEN s.ruleset_id = id THEN 1 END ASC
								) AS rs), '{}');
		
		COMMIT TRANSACTION;

		DROP TABLE IF EXISTS #screen;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			THROW;
	END CATCH
END;