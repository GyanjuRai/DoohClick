using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class PlaylistItem
{
    public int Id { get; set; }

    public int PlaylistId { get; set; }

    public int MediaId { get; set; }

    public int PlaySequence { get; set; }

    public int? DurationOverrideSec { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual MediaLibrary Media { get; set; } = null!;

    public virtual CampaignPlaylist Playlist { get; set; } = null!;
}
