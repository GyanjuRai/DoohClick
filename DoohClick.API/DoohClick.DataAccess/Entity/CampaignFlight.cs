using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class CampaignFlight
{
    public int Id { get; set; }

    public int CampaignId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Campaign Campaign { get; set; } = null!;

    public virtual ICollection<CampaignFlightScreen> CampaignFlightScreens { get; set; } = new List<CampaignFlightScreen>();

    public virtual User? DeletedByNavigation { get; set; }
}
