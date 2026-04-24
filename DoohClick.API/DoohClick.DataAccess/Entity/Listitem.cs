using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Listitem
{
    public int Id { get; set; }

    public int? TenantId { get; set; }

    public int CategoryId { get; set; }

    public string Item { get; set; } = null!;

    public string Code { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ListitemCategory Category { get; set; } = null!;

    public virtual Tenant? Tenant { get; set; }
}
