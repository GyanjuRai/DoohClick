using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Listitem
{
    public int Id { get; set; }

    public string Item { get; set; } = null!;

    public string Value { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual ListitemCategory Category { get; set; } = null!;
}
