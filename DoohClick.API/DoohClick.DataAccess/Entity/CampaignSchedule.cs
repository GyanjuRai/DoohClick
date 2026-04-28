using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class CampaignSchedule
{
    public int Id { get; set; }

    public int CampaignId { get; set; }

    public int ScreenId { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public decimal RatePerHour { get; set; }

    public decimal? TotalHours { get; set; }

    public decimal? TotalAmount { get; set; }

    public string Currency { get; set; } = null!;

    public int? EstimatedImpressions { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Campaign Campaign { get; set; } = null!;

    public virtual ICollection<CampaignPlaylist> CampaignPlaylists { get; set; } = new List<CampaignPlaylist>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Screen Screen { get; set; } = null!;
}
