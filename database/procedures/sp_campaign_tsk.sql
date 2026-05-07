
/*

=========================================================================================
- Description: Insert campaign detail. Nest sp_campaign_flight_ins if "id" is NULL; else
                sp_campaign_flight_del. Nest sp_campaign_flight_screen_ins and
                sp_campaign_flight_screen_del.
- Author: Gyanju Rai
- Created: 2026-05-07
==========================================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                    "Id": 2,
                                    "CampaignCode": "CAMP-LAMAR-2026Q2",
                                    "TenantId": 1,
                                    "AdvertiserId": 2,
                                    "Name": "Amazon Prime Day Awareness 2026",
                                    "Status": "DRAFT",
                                    "StartDate": "2026-06-01",
                                    "EndDate": "2026-08-31",
                                    "Remarks": "Prime Day countdown and awareness campaign",
                                    "CreatedBy": 1,
                                    "ModifiedBy": 1,
                                    "CampaignFlight": [
                                                {
                                                    "Id": null,
                                                    "StartDate": "2026-06-01",
                                                    "EndDate": "2026-06-15",
                                                    "Screens": [
                                                        { 
                                                            "Id":  null,
                                                            "CampaignFlightId": 5,
                                                            "ScreenId": 1 
                                                                
                                                        }   
                                                    ]
                                                },
                                                {
                                                    "Id": 5,
                                                    "StartDate": "2026-06-16",
                                                    "EndDate": "2026-06-30",
                                                    "Screens": [
                                                        { 
                                                            "Id": null,
                                                            "CampaignFlightId": 5,
                                                            "ScreenId": 2
                                                        }   
                                                    ]
                                                },
                                                {
                                                  "Id": 4,
                                                  "StartDate": "2026-07-01",
                                                  "EndDate": "2026-07-15",
                                                  "Screens": [
                                                        { 
                                                            "Id": 1,
                                                            "CampaignFlightId": 5,
                                                            "ScreenId": 1 
                                                        }   
                                                    ]
                                                }
                                    ]
                                }';

EXEC dbo.sp_campaign_tsk @Json = @Json OUTPUT;

SELECT @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_campaign_tsk
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
SET NOCOUNT ON
    BEGIN TRY
        BEGIN TRANSACTION

        DECLARE @CampaignId INT;

        CREATE TABLE #inserted
        (
            id INT NOT NULL,
            campaign_code NVARCHAR(50) NOT NULL
        );

        CREATE TABLE #campaign
        (
            id                      INT,
            campaign_code           NVARCHAR(50),
            tenant_id               INT,
            advertiser_id           INT,
            [name]                  NVARCHAR(150),
            [status]                NVARCHAR(50),
            [start_date]            DATE,
            end_date                DATE,
            remarks                 NVARCHAR(255),
            created_by              INT,
            modified_by             INT NULL,
            campaign_flight         NVARCHAR(MAX),
        );

        CREATE TABLE #campaign_flight
        (
            id INT NULL,
            campaign_id INT NOT NULL,
            [start_date] DATE NOT NULL,
	        end_date DATE NOT NULL,
            deleted_by INT NULL,
            screens NVARCHAR(MAX)
        );

        INSERT INTO #campaign
        (
            id, 
            campaign_code, 
            tenant_id, 
            advertiser_id,
            [name], 
            [status], 
            [start_date], 
            end_date,
            remarks, 
            created_by, 
            campaign_flight
        )
        SELECT  oj.Id,
                oj.CampaignCode,
                oj.TenantId,
                oj.AdvertiserId,
                oj.[Name],
                oj.[Status],
                oj.StartDate,
                oj.EndDate,
                oj.Remarks,
                oj.CreatedBy,
                oj.CampaignFlight
        FROM OPENJSON(@Json)
        WITH
        (
            Id                      INT,
            CampaignCode            NVARCHAR(50),
            TenantId                INT,
            AdvertiserId            INT,
            [Name]                  NVARCHAR(150),
            [Status]                NVARCHAR(50),
            StartDate               DATE,
            EndDate                 DATE,
            Remarks                 NVARCHAR(255),
            CreatedBy               INT,
            ModifiedBy              INT,
            CampaignFlight          NVARCHAR(MAX)   AS JSON
        ) AS oj;

        INSERT INTO dbo.campaign
        (
            campaign_code,
            tenant_id,
            advertiser_id,
            [name],
            [status],
            [start_date],
            end_date,
            remarks,
            created_by
        )
        OUTPUT Inserted.id, Inserted.campaign_code INTO #inserted ( id, campaign_code )
        SELECT  campaign_code,
                tenant_id,
                advertiser_id,
                [name],
                [status],
                [start_date],
                end_date,
                remarks,
                created_by
        FROM #campaign WHERE id IS NULL;

        UPDATE  c
        SET c.[name] = tc.[name],
            c.[start_date] = tc.[start_date],
            c.end_date = tc.end_date,
            c.remarks = tc.remarks,
            c.modified_by = tc.modified_by,
            c.modified_at = GETUTCDATE()
        FROM dbo.campaign AS c
        INNER JOIN #campaign AS tc ON c.id = tc.id AND tc.[status] = 'DRAFT';

        -- Updating with new id
        UPDATE tc
        SET tc.id = i.id
        FROM #campaign tc
        INNER JOIN #inserted AS i ON tc.campaign_code = i.campaign_code;

        -- campaign flight
        INSERT INTO #campaign_flight
        (
            id,
            campaign_id,
            [start_date],
            end_date,
            deleted_by,
            screens
        )
        SELECT  ca.Id,
                tc.id AS CampaignId,
                ca.StartDate,
                ca.EndDate,
                ca.DeletedBy,
                ca.Screens
        FROM #campaign AS tc
        CROSS APPLY
        (
            SELECT  Id,
                    StartDate,
                    EndDate,
                    DeletedBy,
                    Screens
            FROM OPENJSON(tc.campaign_flight)
            WITH
            (
                Id          INT,
                StartDate   DATE,
                EndDate     DATE,
                DeletedBy   INT,
                Screens     NVARCHAR(MAX) AS JSON
            )
        ) AS ca;

        
        DECLARE @CampaignFlightInsJson NVARCHAR(MAX) = ISNULL((
                        SELECT  tcf.campaign_id AS CampaignId,
                                tcf.[start_date] AS StartDate,
                                tcf.end_date AS EndDate
                        FROM #campaign_flight AS tcf
                        WHERE tcf.Id IS NULL
                        FOR JSON PATH,
                        INCLUDE_NULL_VALUES
                    ), '[]');

        EXEC dbo.sp_campaign_flight_ins @Json = @CampaignFlightInsJson OUT;

        UPDATE  tcf 
        SET tcf.id = ins.id
        FROM #campaign_flight As tcf
        INNER JOIN OPENJSON(@CampaignFlightInsJson)
        WITH (
                id INT, 
                [start_date] DATE,
                end_date DATE 
        ) AS ins 
        ON  tcf.[start_date] = ins.[start_date] AND
            tcf.end_date = ins.end_date AND
            tcf.id IS NULL;

        SET @CampaignId = (SELECT   DISTINCT 
                                    campaign_id 
                            FROM #campaign_flight
                            );

        DECLARE @CampaignFlightDelJson NVARCHAR(MAX) = ISNULL((
                            SELECT  cf.id AS [Id],
                                    tc.modified_by AS DeletedBy
                            FROM dbo.campaign_flight AS cf
                            INNER JOIN #campaign AS tc ON cf.campaign_id = @CampaignId
                            WHERE cf.campaign_id = @CampaignId AND
                            NOT EXISTS (
                                SELECT 1 FROM #campaign_flight AS tcf
                                WHERE tcf.id = cf.id
                            )
                            FOR JSON PATH,
                            INCLUDE_NULL_VALUES
                        ), '[]');

        IF @CampaignFlightDelJson != '[]'
        BEGIN
            EXEC dbo.sp_campaign_flight_del @Json = @CampaignFlightDelJson OUT;
            
        END;
        -- campaign screen
        --SELECT *
        --FROM #campaign_flight AS tcf
        --CROSS APPLY
        --(
        --    SELECT  
        --    FROM OPENJSON()
        --) AS ca;

        ROLLBACK TRANSACTION;

        DROP TABLE IF EXISTS #campaign, #campaign_flight, #inserted;
    END TRY
    BEGIN CATCH
        
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH
END