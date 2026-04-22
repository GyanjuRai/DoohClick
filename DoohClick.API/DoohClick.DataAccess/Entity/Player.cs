using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Player
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid TenantId { get; set; }

    public int Status { get; set; }

    public int DeviceType { get; set; }

    public string? HardwareId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

    public DateTime? LastHeartbeatAt { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<Screen> Screens { get; set; } = new List<Screen>();

    public virtual User? UpdatedByNavigation { get; set; }
}
