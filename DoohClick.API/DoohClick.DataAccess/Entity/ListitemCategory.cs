using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class ListitemCategory
{
    public int Id { get; set; }

    public int? TenantId { get; set; }

    public string Category { get; set; } = null!;

    public string CategoryCode { get; set; } = null!;

    public virtual ICollection<Listitem> Listitems { get; set; } = new List<Listitem>();

    public virtual Tenant? Tenant { get; set; }
}
