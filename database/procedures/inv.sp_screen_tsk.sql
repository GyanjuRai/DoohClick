
/*

=====================================================================
- Description: Upsert a screen record with its
               operating hours and supported media
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                    "Id": 1,
                                    "TenantId": 1,
                                    "Name": "Times Square Billboard A",
                                    "ScreenCode": "TIM-LAM-02021",
                                    "Description": "High-traffic digital billboard at Times Square intersection",
                                    "DefaultResolution": "1920x1080",
                                    "Orientation": "Landscape",
                                    "Location": "Times Square, New York",
                                    "AddressLine": "1560 Broadway, New York, NY 10036",
                                    "Tag": "[\"outdoor\",\"premium\",\"high-traffic\"]",
                                    "CountryCode": "US",
                                    "City": "NEWYORK",
                                    "Timezone": "AMERICA/NEWYORK",
                                    "IsActive": true,
                                    "RatePerHour": 100000.00,
                                    "Currency": "USD",
                                    "UserId": 1,
                                    "OperatingHour": [
                                    {
                                        "Id": null,
                                        "DayOfWeek": "SUNDAY",
                                        "OpenTime": "06:00",
                                        "CloseTime": "23:00",
                                        "AudienceSource": "FOOTTRAFFICSENSOR",
                                        "EstimatedImpression": 5000000
                                    },
                                    {
                                        "Id": null,
                                        "DayOfWeek": "SATURDAY",
                                        "OpenTime": "08:00",
                                        "CloseTime": "00:00",
                                        "AudienceSource": "FOOTTRAFFICSENSOR",
                                        "EstimatedImpression": 7500000
                                    }
                                    ],
                                    "SupportedMedia": [
                                    {
                                        "MediaType": "MP4"
                                    },
                                    {
                                        "MediaType": "JPEG"
                                    }
                                    ]
                                }';

EXEC inv.sp_screen_tsk @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE inv.sp_screen_tsk
(
	@Json NVARCHAR(MAX) OUT
)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY

        CREATE TABLE #inserted
        (
            id INT NOT NULL,
            screen_code NVARCHAR(40) NOT NULL
        );

        CREATE TABLE #screen
        (
            id                  INT NULL,
	        tenant_id			INT NOT NULL,
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
	        [user_id]           INT NOT NULL,
            operating_hour      NVARCHAR(MAX) NULL,
            supported_media     NVARCHAR(MAX) NULL,

	        created_at          DATETIME2 NOT NULL,
	        updated_at          DATETIME2 NULL
        );

        INSERT INTO #screen
        (
            id,
            tenant_id,
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
            [user_id],
            operating_hour,
            supported_media,
            created_at,
            updated_at
        )
        SELECT  oj.Id,
                oj.TenantId,
                oj.[Name],
                LOWER(oj.[Name]),
                oj.ScreenCode,
                oj.[Description],
                oj.DefaultResolution,
                oj.Orientation,
                oj.[Location],
                oj.AddressLine,
                oj.Tag,
                oj.CountryCode,
                oj.City,
                oj.Timezone,
                oj.IsActive,
                oj.RatePerHour,
                oj.Currency,
                oj.UserId,
                oj.OperatingHour,
                oj.SupportedMedia,
                GETUTCDATE(),
                GETUTCDATE()
        FROM OPENJSON(@Json)
        WITH
        (
            Id INT,
            TenantId INT,
            [Name] NVARCHAR(100),
            ScreenCode NVARCHAR(40),
            [Description] NVARCHAR(500),
            DefaultResolution  NVARCHAR(50),
            Orientation NVARCHAR(50),
            [Location] NVARCHAR(100),
            AddressLine NVARCHAR(200),
            Tag NVARCHAR(MAX),
            CountryCode NVARCHAR(50),
            City NVARCHAR(50),
            Timezone NVARCHAR(50),
            IsActive BIT,
            RatePerHour DECIMAL(10,2),
            Currency NVARCHAR(50),
            UserId  INT,
            OperatingHour NVARCHAR(MAX) AS JSON,
            SupportedMedia NVARCHAR(MAX) AS JSON
        ) AS oj;

    BEGIN TRANSACTION

    INSERT INTO inv.screen
    (
        tenant_id,
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
        created_by,
        created_at
    )
    OUTPUT Inserted.id, Inserted.screen_code INTO #inserted ( id, screen_code )
    SELECT 
            ts.tenant_id,
            ts.[name],
            ts.normalized_name,
            ts.screen_code,
            ts.[description],
            ts.default_resolution,
            ts.orientation,
            ts.[location],
            ts.address_line,
            ts.tag,
            ts.country_code,
            ts.city,
            ts.timezone,
            ts.is_active,
            ts.rate_per_hour,
            ts.currency,
            ts.[user_id],
            ts.created_at
    FROM #screen AS ts WHERE ts.id IS NULL OR ts.id = 0;

    UPDATE s
    SET s.[name] = ts.[name],
        s.normalized_name = ts.normalized_name,
        s.[description] = ts.[description],
        s.[location] = ts.[location],
        s.address_line = ts.address_line,
        s.tag = ts.tag,
        s.is_active = ts.is_active,
        s.rate_per_hour = ts.rate_per_hour,
        s.updated_by = ts.[user_id],
        s.updated_at = ts.updated_at
    FROM inv.screen AS s
    INNER JOIN #screen AS ts ON s.id = ts.id;

    UPDATE ts
    SET ts.id = i.id
    FROM #screen AS ts
    INNER JOIN #inserted AS i ON ts.screen_code = i.screen_code;

    DECLARE @ScreenOperatingHrJson NVARCHAR(MAX) = ISNULL((
			SELECT  ca.*,
				    s.id AS ScreenId,
                    ts.[user_id] AS [CreatedBy]
			FROM inv.screen AS s
			INNER JOIN #screen AS ts ON s.id = ts.id
			CROSS APPLY (
				SELECT *
				FROM OPENJSON(ts.operating_hour) WITH (
						Id INT,
						[DayOfWeek] NVARCHAR(50),
						OpenTime TIME,
						CloseTime TIME,
						AudienceSource NVARCHAR(50),
						EstimatedImpression INT
						)
				) AS ca
			FOR JSON PATH,
				INCLUDE_NULL_VALUES
			), '[]');

    EXEC inv.sp_screen_opr_hrs_ins @Json = @ScreenOperatingHrJson OUT;

    DECLARE @ScreenSupMeidaJson NVARCHAR(MAX) = ISNULL((
			SELECT  s.id AS [ScreenId],
				    ca.*,
				    ts.[user_id] AS [CreatedBy]
			FROM inv.screen AS s
			INNER JOIN #screen AS ts ON s.id = ts.id
			CROSS APPLY (
				SELECT *
				FROM OPENJSON(ts.supported_media) WITH (MediaType NVARCHAR(50))
				) AS ca
			FOR JSON PATH,
				INCLUDE_NULL_VALUES
			), '[]');

    EXEC inv.sp_screen_sup_media_ins @Json = @ScreenSupMeidaJson OUT;

    COMMIT TRANSACTION

   SELECT @Json = ISNULL((
                        SELECT  s.id,
                                s.tenant_id,
                                t.[name]        AS tenant_name,
                                s.[name],
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
                        INNER JOIN #screen               AS ts ON s.id          = ts.id
                        INNER JOIN [identity].tenant     AS t  ON s.tenant_id   = t.id
                        INNER JOIN [identity].[user]     AS cu ON s.created_by  = cu.id
                        LEFT  JOIN [identity].[user]     AS uu ON s.updated_by  = uu.id
                        FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
                        ), '{}');

    DROP TABLE #screen, #inserted;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DROP TABLE IF EXISTS #inserted, #screen;

        THROW;

	END CATCH
END