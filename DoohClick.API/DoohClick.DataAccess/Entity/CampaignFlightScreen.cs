using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class CampaignFlightScreen
{
    public int Id { get; set; }

    public int CampaignFlightId { get; set; }

    public int ScreenId { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual CampaignFlight CampaignFlight { get; set; } = null!;

    public virtual ICollection<CampaignScreenSchedule> CampaignScreenSchedules { get; set; } = new List<CampaignScreenSchedule>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Screen Screen { get; set; } = null!;
}
