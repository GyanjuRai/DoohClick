
/*

=====================================================================
- Description: insert screen schedule and playlist.
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'[
                                  {
                                    "Id": 1,
                                    "CampaignFlightScreenId": 3,
                                    "DayOfWeek": "MONDAY",
                                    "StartTime": "08:00:00",
                                    "EndTime": "20:00:00",
                                    "CreatedBy": 1,
                                    "DeletedBy": null,
                                    "PlaylistItem": [
                                      { "Id": 1, "MediaId": 1, "PlayOrder": 1, "DurationSeconds": 15 },
                                      { "Id": 2, "MediaId": 2, "PlayOrder": 2, "DurationSeconds": 30 }
                                    ]
                                  }
                                ]';

EXEC dbo.sp_campaign_screen_schedule_tsk @Json = @Json OUT;

SELECT @Json;

*/
CREATE OR ALTER PROCEDURE dbo.sp_campaign_screen_schedule_tsk
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
    SET NOCOUNT ON
    BEGIN TRY
    BEGIN TRANSACTION

        DECLARE @CampaignFlightScreenId INT;

        CREATE TABLE #inserted
        (
            inserted_id INT,
            id          INT IDENTITY(1,1)
        );

        CREATE TABLE #inserted_playlist
        (
            inserted_id INT,
            id          INT IDENTITY(1,1)
        );

        CREATE TABLE #campaign_screen_schedule
        (
            id                        INT IDENTITY(1,1),
            schedule_id               INT,
            campaign_flight_screen_id INT,
            day_of_week               NVARCHAR(50),
            start_time                TIME,
            end_time                  TIME,
            created_by                INT,
            deleted_by                INT NULL,
            playlist_item             NVARCHAR(MAX)
        );

        CREATE TABLE #campaign_playlist
        (
            id               INT IDENTITY(1,1),
            playlist_id      INT,
            schedule_id      INT,
            media_id         INT,
            play_order       INT,
            duration_seconds INT,
            created_by       INT,
            deleted_by       INT NULL
        );

        INSERT INTO #campaign_screen_schedule
        (
            schedule_id,
            campaign_flight_screen_id,
            day_of_week,
            start_time,
            end_time,
            created_by,
            deleted_by,
            playlist_item
        )
        SELECT  oj.Id,
                oj.CampaignFlightScreenId,
                oj.DayOfWeek,
                oj.StartTime,
                oj.EndTime,
                oj.CreatedBy,
                oj.DeletedBy,
                oj.PlaylistItem
        FROM OPENJSON(@Json)
        WITH
        (
            Id                      INT,
            CampaignFlightScreenId  INT,
            DayOfWeek               NVARCHAR(50),
            StartTime               TIME,
            EndTime                 TIME,
            CreatedBy               INT,
            DeletedBy               INT,
            PlaylistItem            NVARCHAR(MAX) AS JSON
        ) AS oj;


        SET @CampaignFlightScreenId = (SELECT TOP 1 campaign_flight_screen_id FROM #campaign_screen_schedule);

        INSERT INTO dbo.campaign_screen_schedule
        (
            campaign_flight_screen_id,
            day_of_week,
            start_time,
            end_time,
            created_by
        )
        OUTPUT Inserted.id INTO #inserted (inserted_id)
        SELECT  campaign_flight_screen_id,
                day_of_week,
                start_time,
                end_time,
                created_by
        FROM #campaign_screen_schedule
        WHERE schedule_id IS NULL;

        UPDATE css
        SET css.is_deleted = 1,
            css.deleted_by = tcss.deleted_by,
            css.deleted_at = GETUTCDATE()
        FROM dbo.campaign_screen_schedule AS css
        INNER JOIN #campaign_screen_schedule AS tcss ON css.id = tcss.schedule_id
        WHERE tcss.deleted_by IS NOT NULL;

        -- Cascade: soft-delete playlist items of deleted schedules
        UPDATE cpi
        SET cpi.is_deleted = 1,
            cpi.deleted_by = tcss.deleted_by,
            cpi.deleted_at = GETUTCDATE()
        FROM dbo.campaign_playlist_item AS cpi
        INNER JOIN #campaign_screen_schedule AS tcss ON cpi.schedule_id = tcss.schedule_id
        WHERE tcss.deleted_by IS NOT NULL
          AND cpi.is_deleted = 0;

        UPDATE tcss
        SET tcss.schedule_id = i.inserted_id
        FROM #campaign_screen_schedule AS tcss
        INNER JOIN #inserted AS i ON tcss.id = i.id;

        INSERT INTO #campaign_playlist
        (
            playlist_id,
            schedule_id,
            media_id,
            play_order,
            duration_seconds,
            created_by,
            deleted_by
        )
        SELECT  pl.Id,
                tcss.schedule_id,
                pl.MediaId,
                pl.PlayOrder,
                pl.DurationSeconds,
                tcss.created_by,
                tcss.deleted_by
        FROM #campaign_screen_schedule AS tcss
        CROSS APPLY OPENJSON(tcss.playlist_item)
        WITH
        (
            Id              INT,
            MediaId         INT,
            PlayOrder       INT,
            DurationSeconds INT
        ) AS pl
        WHERE tcss.deleted_by IS NULL;

        INSERT INTO dbo.campaign_playlist_item
        (
            schedule_id,
            media_id,
            play_order,
            duration_seconds,
            created_by
        )
        OUTPUT Inserted.id INTO #inserted_playlist (inserted_id)
        SELECT  schedule_id,
                media_id,
                play_order,
                duration_seconds,
                created_by
        FROM #campaign_playlist
        WHERE playlist_id IS NULL;

        UPDATE tpi
        SET tpi.playlist_id = i.inserted_id
        FROM #campaign_playlist AS tpi
        INNER JOIN #inserted_playlist AS i ON tpi.id = i.id
        WHERE tpi.playlist_id IS NULL;

        UPDATE cpi
        SET cpi.is_deleted = 1,
            cpi.deleted_by = tcss.created_by, -- Modifier 
            cpi.deleted_at = GETUTCDATE()
        FROM dbo.campaign_playlist_item AS cpi
        INNER JOIN #campaign_screen_schedule AS tcss ON cpi.schedule_id = tcss.schedule_id
        WHERE tcss.deleted_by IS NULL
          AND cpi.is_deleted = 0
          AND cpi.id NOT IN
          (
              SELECT tpi.playlist_id
              FROM #campaign_playlist AS tpi
              WHERE tpi.schedule_id = cpi.schedule_id
          );

        -- ============
        -- OUTPUT
        -- ============

        WITH campaign_flights AS
        (
            SELECT  cf.id,
                    cf.start_date,
                    cf.end_date
            FROM dbo.campaign_flight_screen AS cfs
            INNER JOIN dbo.campaign_flight AS cf ON cf.id = cfs.campaign_flight_id
            WHERE cfs.id = @CampaignFlightScreenId
              AND cf.is_deleted = 0
        )
        SELECT @Json = ISNULL((
            SELECT  cf.id                   AS flight_id,
                    cfs.id                  AS campaign_flight_screen_id,
                    cf.[start_date],
                    cf.end_date,
                    s.id                    AS screen_id,
                    s.[name]                AS screen_name,
                    s.default_resolution    AS screen_resolution,
                    s.country_code          AS screen_country,
                    s.city                  AS screen_city,
                    ISNULL(
                        JSON_QUERY((
                            SELECT  css.id,
                                    css.campaign_flight_screen_id,
                                    css.start_time,
                                    css.end_time,
                                    css.day_of_week,
                                    ISNULL(
                                        JSON_QUERY((
                                            SELECT  cpi.id,
                                                    cpi.duration_seconds,
                                                    cpi.media_id,
                                                    cpi.play_order,
                                                    ml.display_name,
                                                    ml.file_url,
                                                    ml.file_size_bytes
                                            FROM dbo.campaign_playlist_item AS cpi
                                            INNER JOIN dbo.media_library AS ml ON cpi.media_id = ml.id
                                            WHERE   cpi.schedule_id = css.id
                                                AND cpi.is_deleted = 0
                                                AND ml.is_deleted = 0
                                            FOR JSON PATH, INCLUDE_NULL_VALUES
                                        )),
                                    '[]') AS playlist
                            FROM dbo.campaign_screen_schedule AS css
                            WHERE   css.campaign_flight_screen_id = cfs.id
                                AND css.is_deleted = 0
                            FOR JSON PATH, INCLUDE_NULL_VALUES
                        )),
                    '[]') AS campaign_screen_schedules
            FROM campaign_flights AS cf
            INNER JOIN dbo.campaign_flight_screen AS cfs ON cfs.campaign_flight_id = cf.id
            INNER JOIN inv.screen AS s ON cfs.screen_id = s.id
            WHERE cfs.id = @CampaignFlightScreenId
            FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER
        ), '[]');

        COMMIT TRANSACTION;

        DROP TABLE IF EXISTS #campaign_screen_schedule, #inserted, #campaign_playlist;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END