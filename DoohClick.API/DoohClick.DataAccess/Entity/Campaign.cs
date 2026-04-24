using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Campaign
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public int TenantId { get; set; }

    public int AdvertiserId { get; set; }

    public string Name { get; set; } = null!;

    public string CampaignCode { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Note { get; set; }

    public bool? IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Advertiser Advertiser { get; set; } = null!;

    public virtual ICollection<CampaignSchedule> CampaignSchedules { get; set; } = new List<CampaignSchedule>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
