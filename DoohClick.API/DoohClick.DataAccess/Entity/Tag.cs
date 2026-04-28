using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Tag
{
    public int Id { get; set; }

    public int TenantId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
