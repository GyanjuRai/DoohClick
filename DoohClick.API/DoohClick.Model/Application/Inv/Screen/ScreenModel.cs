

namespace DoohClick.Model.Application.Inv.Screen
{
    public record MvScreenFilterOptions
    {
        public required int TenantId { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string>? CountryCodeList { get; set; }
        public List<string>? CityList { get; set; }
        public List<string>? OrientationList { get; set; }
        public List<string>? ResolutionList { get; set; }
    }
    public record MvScreen
    {
        public int? Id { get; set; }
        public Guid? Uuid { get; set; }
        public int? TenantId { get; set; }
        public string? TenantName { get; set; }
        public required string Name { get; set; }
        public string? NormalizedName { get; set; }
        public required string ScreenCode { get; set; }
        public string? Description { get; set; }
        public required string DefaultResolution { get; set; }
        public required string Orientation { get; set; }
        public required string Location { get; set; }
        public string? AddressLine { get; set; }
        public List<string>? Tag { get; set; }
        public required string CountryCode { get; set; }
        public required string City { get; set; }
        public required string Timezone { get; set; }
        public bool IsActive { get; set; }
        public decimal RatePerHour { get; set; }
        public required string Currency { get; set; }
        public List<MvScreenOperatingHour>? OperatingHour { get; set; } = [];
        public List<MvScreenSupportedMedia>? SupportedMedia { get; set; } = [];
        public int? CreatedBy { get; set; }
        public string? Creator { get; set; }
        public int? UpdatedBy { get; set; }
        public string? Modifier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public record MvScreenOperatingHour
    {
        public int? Id { get; set; }
        public required string DayOfWeek { get; set; }
        public required string OpenTime { get; set; }
        public required string CloseTime { get; set; }
        public required string AudienceSource { get; set; }
        public int? EstimatedImpression { get; set; }
    }

    public record MvScreenSupportedMedia
    {
        public int? Id { get; set; }
        public required string MediaType { get; set; }
    }

    public record MvScreenDelParam
    {
        public required Guid Uuid { get; set; }
        public required int TenantId { get; set; }
        public required int DeletedBy { get; set; }
    }
}
