

namespace DoohClick.Model.Application.crm.advertiser
{
    public record MvAdvertiser
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string ContactName { get; set; }
        public required string ContactEmail { get; set; }
        public required string ContactPhone { get; set; }
        public required bool? IsActive { get; set; }
        public int? CreatedBy{ get; set; }
        public int? UpdatedBy{ get;set; }
        public string? Creator { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public record MvAdvertiserDdl
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
    }

    public record MvTenantIdParam
    {
        public int? TenantId { get; set; }
    }

    public record MvAdvertiserFilterOptions
    {
        public int? TenantId { get; set; }
        public List<bool>? IsActiveList { get; set; }
    }
}
