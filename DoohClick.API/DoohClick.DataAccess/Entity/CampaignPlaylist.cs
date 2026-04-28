using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class CampaignPlaylist
{
    public int Id { get; set; }

    public int CampaignScheduleId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int Version { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual CampaignSchedule CampaignSchedule { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();

    public virtual User? UpdatedByNavigation { get; set; }
}
