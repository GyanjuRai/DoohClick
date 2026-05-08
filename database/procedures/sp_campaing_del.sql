
/*

=====================================================================
- Description: Soft delete campaign and underlying all .
- Author: Gyanju Rai
- Created: 2026-04-24
=====================================================================

DECLARE @Json NVARCHAR(MAX) = N'{
                                    "Id": 2,
                                    "DeletedBy": 1
                                }';

EXEC dbo.sp_campaing_del @Json = @Json;

*/

CREATE OR ALTER PROCEDURE dbo.sp_campaing_del
(
    @Json NVARCHAR(MAX) OUT
)
AS
BEGIN
    SET NOCOUNT ON
    DECLARE @Id INT = ISNULL(JSON_VALUE(@Json, '$.Id'), 0),
            @DeletedBy INT = JSON_VALUE(@Json, '$.DeletedBy');

    BEGIN TRANSACTION

    UPDATE  c
    SET c.is_deleted = 1,
        c.deleted_by = @DeletedBy,
        c.deleted_at = GETUTCDATE()
    FROM dbo.campaign AS c
    WHERE c.id = @Id;
    
    DECLARE @CampaingFlightDelJson NVARCHAR(MAX) = ISNULL((
        SELECT  cf.id AS Id,
                @DeletedBy AS DeletedBy
        FROM dbo.campaign_flight AS cf WHERE cf.campaign_id = @Id
        AND cf.is_deleted = 0
        FOR JSON PATH
    ),'[]');

    EXEC dbo.sp_campaign_flight_del @Json = @CampaingFlightDelJson OUT;

    DECLARE @CampaignFlightScreenDelJson NVARCHAR(MAX) = ISNULL((
        SELECT  cfs.id AS Id,
                @DeletedBy AS DeletedBy
        FROM dbo.campaign_flight_screen AS cfs
        INNER JOIN dbo.campaign_flight AS cf ON cfs.campaign_flight_id = cf.id
        WHERE cf.campaign_id = @Id
        AND cfs.is_deleted = 0        
        FOR JSON PATH               
    ), '[]');

    EXEC dbo.sp_campaign_flight_screen_del @Json = @CampaignFlightScreenDelJson OUT;

    SELECT @Json = ISNULL((
        SELECT id
        FROM dbo.campaign 
        WHERE id = @Id
        FOR JSON PATH, 
        WITHOUT_ARRAY_WRAPPER
    ), '[]');

    ROLLBACK TRANSACTION;
END