using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class ListitemCategory
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Listitem> Listitems { get; set; } = new List<Listitem>();
}
