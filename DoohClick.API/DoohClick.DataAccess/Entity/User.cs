using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string NormalizedUserName { get; set; } = null!;

    public string? Name { get; set; }

    public string? SurName { get; set; }

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string SecurityStamp { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid TenantId { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseDeletedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<Player> PlayerApprovedByNavigations { get; set; } = new List<Player>();

    public virtual ICollection<Player> PlayerCreatedByNavigations { get; set; } = new List<Player>();

    public virtual ICollection<Player> PlayerDeletedByNavigations { get; set; } = new List<Player>();

    public virtual ICollection<Player> PlayerUpdatedByNavigations { get; set; } = new List<Player>();

    public virtual ICollection<Ruleset> RulesetApprovedByNavigations { get; set; } = new List<Ruleset>();

    public virtual ICollection<Ruleset> RulesetCreatedByNavigations { get; set; } = new List<Ruleset>();

    public virtual ICollection<Ruleset> RulesetDeletedByNavigations { get; set; } = new List<Ruleset>();

    public virtual ICollection<Ruleset> RulesetUpdatedByNavigations { get; set; } = new List<Ruleset>();

    public virtual ICollection<Screen> ScreenApprovedByNavigations { get; set; } = new List<Screen>();

    public virtual ICollection<Screen> ScreenCreatedByNavigations { get; set; } = new List<Screen>();

    public virtual ICollection<Screen> ScreenDeletedByNavigations { get; set; } = new List<Screen>();

    public virtual ICollection<ScreenGroup> ScreenGroupCreatedByNavigations { get; set; } = new List<ScreenGroup>();

    public virtual ICollection<ScreenGroup> ScreenGroupDeletedByNavigations { get; set; } = new List<ScreenGroup>();

    public virtual ICollection<ScreenGroup> ScreenGroupUpdatedByNavigations { get; set; } = new List<ScreenGroup>();

    public virtual ICollection<Screen> ScreenUpdatedByNavigations { get; set; } = new List<Screen>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual ICollection<Tenant> TenantCreatedByNavigations { get; set; } = new List<Tenant>();

    public virtual ICollection<Tenant> TenantDeletedByNavigations { get; set; } = new List<Tenant>();

    public virtual ICollection<Tenant> TenantUpdatedByNavigations { get; set; } = new List<Tenant>();

    public virtual User? UpdatedByNavigation { get; set; }
}
