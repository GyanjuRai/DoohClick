using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class User
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public int TenantId { get; set; }

    public string UserName { get; set; } = null!;

    public string NormalizedUserName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string SurName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string UserRole { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiry { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Advertiser> AdvertiserCreatedByNavigations { get; set; } = new List<Advertiser>();

    public virtual ICollection<Advertiser> AdvertiserDeletedByNavigations { get; set; } = new List<Advertiser>();

    public virtual ICollection<Advertiser> AdvertiserUpdatedByNavigations { get; set; } = new List<Advertiser>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<Campaign> CampaignCreatedByNavigations { get; set; } = new List<Campaign>();

    public virtual ICollection<Campaign> CampaignDeletedByNavigations { get; set; } = new List<Campaign>();

    public virtual ICollection<CampaignPlaylist> CampaignPlaylistCreatedByNavigations { get; set; } = new List<CampaignPlaylist>();

    public virtual ICollection<CampaignPlaylist> CampaignPlaylistDeletedByNavigations { get; set; } = new List<CampaignPlaylist>();

    public virtual ICollection<CampaignPlaylist> CampaignPlaylistUpdatedByNavigations { get; set; } = new List<CampaignPlaylist>();

    public virtual ICollection<CampaignSchedule> CampaignScheduleCreatedByNavigations { get; set; } = new List<CampaignSchedule>();

    public virtual ICollection<CampaignSchedule> CampaignScheduleDeletedByNavigations { get; set; } = new List<CampaignSchedule>();

    public virtual ICollection<Campaign> CampaignUpdatedByNavigations { get; set; } = new List<Campaign>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseDeletedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<MediaLibrary> MediaLibraryCreatedByNavigations { get; set; } = new List<MediaLibrary>();

    public virtual ICollection<MediaLibrary> MediaLibraryDeletedByNavigations { get; set; } = new List<MediaLibrary>();

    public virtual ICollection<MediaLibrary> MediaLibraryUploadedByNavigations { get; set; } = new List<MediaLibrary>();

    public virtual ICollection<PlaylistItem> PlaylistItemCreatedByNavigations { get; set; } = new List<PlaylistItem>();

    public virtual ICollection<PlaylistItem> PlaylistItemDeletedByNavigations { get; set; } = new List<PlaylistItem>();

    public virtual ICollection<Screen> ScreenCreatedByNavigations { get; set; } = new List<Screen>();

    public virtual ICollection<Screen> ScreenDeletedByNavigations { get; set; } = new List<Screen>();

    public virtual ICollection<ScreenOperatingHour> ScreenOperatingHours { get; set; } = new List<ScreenOperatingHour>();

    public virtual ICollection<ScreenSupportedMedium> ScreenSupportedMedia { get; set; } = new List<ScreenSupportedMedium>();

    public virtual ICollection<Screen> ScreenUpdatedByNavigations { get; set; } = new List<Screen>();

    public virtual ICollection<Tag> TagCreatedByNavigations { get; set; } = new List<Tag>();

    public virtual ICollection<Tag> TagUpdatedByNavigations { get; set; } = new List<Tag>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual ICollection<Tenant> TenantCreatedByNavigations { get; set; } = new List<Tenant>();

    public virtual ICollection<Tenant> TenantDeletedByNavigations { get; set; } = new List<Tenant>();

    public virtual ICollection<Tenant> TenantUpdatedByNavigations { get; set; } = new List<Tenant>();

    public virtual User? UpdatedByNavigation { get; set; }
}
