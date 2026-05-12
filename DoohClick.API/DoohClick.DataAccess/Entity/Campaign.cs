using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Campaign
{
    public int Id { get; set; }

    public string CampaignCode { get; set; } = null!;

    public int TenantId { get; set; }

    public int? AdvertiserId { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? DurationInDays { get; set; }

    public string? Remarks { get; set; }

    public bool IsLocked { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Advertiser? Advertiser { get; set; }

    public virtual ICollection<CampaignFlight> CampaignFlights { get; set; } = new List<CampaignFlight>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Tenant Tenant { get; set; } = null!;
}
