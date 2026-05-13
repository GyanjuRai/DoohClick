using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class MediaLibrary
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public int TenantId { get; set; }

    public string DisplayName { get; set; } = null!;

    public string? FileName { get; set; }

    public string? FileUrl { get; set; }

    public string? Resolution { get; set; }

    public string Status { get; set; } = null!;

    public decimal? DurationSec { get; set; }

    public bool? IsVideo { get; set; }

    public bool IsDeleted { get; set; }

    public int? UploadedBy { get; set; }

    public DateTime? UploadedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? FileSizeBytes { get; set; }

    public int? AdvertiserId { get; set; }

    public virtual Advertiser? Advertiser { get; set; }

    public virtual ICollection<CampaignPlaylistItem> CampaignPlaylistItems { get; set; } = new List<CampaignPlaylistItem>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User? UploadedByNavigation { get; set; }
}
