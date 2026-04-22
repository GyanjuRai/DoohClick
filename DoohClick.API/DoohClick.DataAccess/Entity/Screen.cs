using System;
using System.Collections.Generic;

namespace DoohClick.DataAccess.Entity;

public partial class Screen
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public int Status { get; set; }

    public int Tier { get; set; }

    public int Category { get; set; }

    public int? Subcategory { get; set; }

    public int OrientationType { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public string SupportedFormats { get; set; } = null!;

    public int? MaxFilesizeMb { get; set; }

    public int? MaxDurationSeconds { get; set; }

    public bool? SupportsAudio { get; set; }

    public bool? SupportsHtml5 { get; set; }

    public int? Country { get; set; }

    public int? City { get; set; }

    public int? District { get; set; }

    public int? VenueType { get; set; }

    public string? VenueName { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? TimeZone { get; set; }

    public int? CurrencyCode { get; set; }

    public decimal? Amount { get; set; }

    public Guid? PlayerId { get; set; }

    public Guid? RulesetId { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Player? Player { get; set; }

    public virtual Ruleset? Ruleset { get; set; }

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
