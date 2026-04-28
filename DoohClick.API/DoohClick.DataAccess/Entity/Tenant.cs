using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Tenant
{
    public int Id { get; set; }

    public string TenantCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string DefaultCurrency { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string ContactEmail { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public string TimeZone { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Advertiser> Advertisers { get; set; } = new List<Advertiser>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<ListitemCategory> ListitemCategories { get; set; } = new List<ListitemCategory>();

    public virtual ICollection<Listitem> Listitems { get; set; } = new List<Listitem>();

    public virtual ICollection<MediaLibrary> MediaLibraries { get; set; } = new List<MediaLibrary>();

    public virtual ICollection<Screen> Screens { get; set; } = new List<Screen>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
