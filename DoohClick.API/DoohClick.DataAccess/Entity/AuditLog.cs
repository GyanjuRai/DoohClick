using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class AuditLog
{
    public int Id { get; set; }

    public int TenantId { get; set; }

    public string EntityType { get; set; } = null!;

    public Guid EntityId { get; set; }

    public string Action { get; set; } = null!;

    public int ChangedBy { get; set; }

    public DateTime ChangedAt { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public virtual User ChangedByNavigation { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
