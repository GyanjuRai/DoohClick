using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Advertiser
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public int TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string ContactEmail { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
