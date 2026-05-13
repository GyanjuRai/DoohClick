using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class CampaignPlaylistItem
{
    public int Id { get; set; }

    public int ScheduleId { get; set; }

    public int MediaId { get; set; }

    public int PlayOrder { get; set; }

    public int DurationSeconds { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual MediaLibrary Media { get; set; } = null!;

    public virtual CampaignScreenSchedule Schedule { get; set; } = null!;
}
