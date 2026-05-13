using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class ScreenOperatingHour
{
    public int Id { get; set; }

    public int ScreenId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeOnly OpenTime { get; set; }

    public TimeOnly CloseTime { get; set; }

    public string AudienceSource { get; set; } = null!;

    public int? EstimatedImpression { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Screen Screen { get; set; } = null!;
}
