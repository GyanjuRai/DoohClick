using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Tenant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<Screen> Screens { get; set; } = new List<Screen>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
