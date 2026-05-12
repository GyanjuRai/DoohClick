
namespace DoohClick.Model.Application.commercial.campaign
{
    public record MvCampaign
    {
        public int? Id { get; set; }
        public string? CampaignCode { get; set; }
        public required int TenantId { get; set; }
        public required int AdvertiserId { get; set; }
        public string? Advertiser { get; set; }
        public required string Name { get; set; }
        public string? Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? DurationInDays { get; set; }
        public string? Remarks { get; set; }
        public required int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Creator { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? Modifier { get; set; }
        public bool? IsLocked { get; set; }
        public List<MvCampaignFlight>? CampaignFlight { get; set; }
    }

    public record MvCampaignFlight
    { 
        public int? Id { get; set; }
        public int? CampaignId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<MvCampaignFlightScreen>? Screens { get; set; }
    }

    public record MvCampaignFlightScreen
    {
        public int? Id  { get; set; }
        public int? CampaignFlightId { get; set; }
        public required int ScreenId { get; set; }
    }

    public record MvCampaignIdParam
    {
        public required int Id { get; set; }
        public int? DeletedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }

    public record MvCampaignFilterOptionParam
    {
        public required int TenantId { get; set; }
        public required string Status { get; set; }
        public List<int>? AdvertiserIdList { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
