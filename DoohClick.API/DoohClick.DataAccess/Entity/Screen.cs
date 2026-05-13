using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Screen
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public int TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string ScreenCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string DefaultResolution { get; set; } = null!;

    public string Orientation { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string? AddressLine { get; set; }

    public string Tag { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Timezone { get; set; } = null!;

    public bool IsActive { get; set; }

    public decimal RatePerHour { get; set; }

    public string Currency { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<CampaignFlightScreen> CampaignFlightScreens { get; set; } = new List<CampaignFlightScreen>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<ScreenOperatingHour> ScreenOperatingHours { get; set; } = new List<ScreenOperatingHour>();

    public virtual ICollection<ScreenSupportedMedium> ScreenSupportedMedia { get; set; } = new List<ScreenSupportedMedium>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
