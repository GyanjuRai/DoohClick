using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class ScreenSupportedMedium
{
    public int Id { get; set; }

    public int ScreenId { get; set; }

    public string MediaType { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Screen Screen { get; set; } = null!;
}
