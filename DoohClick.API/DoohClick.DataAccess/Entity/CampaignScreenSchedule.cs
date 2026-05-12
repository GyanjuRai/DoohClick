using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class CampaignScreenSchedule
{
    public int Id { get; set; }

    public int CampaignFlightScreenId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual CampaignFlightScreen CampaignFlightScreen { get; set; } = null!;

    public virtual ICollection<CampaignPlaylistItem> CampaignPlaylistItems { get; set; } = new List<CampaignPlaylistItem>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }
}
