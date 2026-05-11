

namespace DoohClick.Model.Application.cms.media
{

    public record MvMedia
    {
        public int? Id { get; set; }
        public required int TenantId { get; set; }
        public int? AdvertiserId {  get; set; }
        public required string DisplayName { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public long? FileSizeBytes { get; set; }
        public string? Resolution { get; set; }
        public string? Status { get; set; }
        public decimal? DurationSec { get; set; }
        public bool? IsVideo { get; set; }
        public int? UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Creator { get; set; }
        public string? Uploader { get; set; }
    }

    public record MvMediaDdl
    {
        public int? Id { get; set; }
        public required string DisplayName { get; set; }
        public string? FileUrl { get; set; }
        public long? FileSizeBytes { get; set; }
    }

    public record MvMediaFilterOptions
    {
        public required int TenantId { get; set; }
        public bool? IsArchieved { get; set; }
        public bool? IsVideo { get; set; }
    }

    public record MvMediaDel
    {
        public required int Id { get; set; }
        public required int TenantId { get; set; }
        public required int DeletedBy { get; set; }
    }
}
