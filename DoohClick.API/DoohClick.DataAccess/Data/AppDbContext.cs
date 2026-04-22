using System;
using System.Collections.Generic;
using DoohClick.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DoohClick.DataAccess.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Listitem> Listitems { get; set; }

    public virtual DbSet<ListitemCategory> ListitemCategories { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Ruleset> Rulesets { get; set; }

    public virtual DbSet<Screen> Screens { get; set; }

    public virtual DbSet<ScreenGroup> ScreenGroups { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Listitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__listitem__3213E83FB1F50A5B");

            entity.ToTable("listitem", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Item)
                .HasMaxLength(256)
                .HasColumnName("item");
            entity.Property(e => e.Value)
                .HasMaxLength(256)
                .HasColumnName("value");

            entity.HasOne(d => d.Category).WithMany(p => p.Listitems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fK_listitem_category");
        });

        modelBuilder.Entity<ListitemCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__listitem__3213E83FB013226E");

            entity.ToTable("listitem_category", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasColumnName("category");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__player__3213E83F282A3F49");

            entity.ToTable("player", "inv");

            entity.HasIndex(e => e.HardwareId, "UQ__player__991394784CFFAF1B").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ApprovedAt)
                .HasColumnType("datetime")
                .HasColumnName("approved_at");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(40)
                .HasColumnName("concurrency_stamp");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DeviceType).HasColumnName("device_type");
            entity.Property(e => e.HardwareId)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("hardware_id");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.LastHeartbeatAt)
                .HasColumnType("datetime")
                .HasColumnName("last_heartbeat_at");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.PlayerApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__player__approved__4F7CD00D");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PlayerCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__player__created___4CA06362");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.PlayerDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__player__deleted___4E88ABD4");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PlayerUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__player__updated___4D94879B");
        });

        modelBuilder.Entity<Ruleset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ruleset__3213E83F85F90EF4");

            entity.ToTable("ruleset", "inv");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ApprovedAt)
                .HasColumnType("datetime")
                .HasColumnName("approved_at");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.BlockedCategories).HasColumnName("blocked_categories");
            entity.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(40)
                .HasColumnName("concurrency_stamp");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.MaxSlotsPerLoop).HasColumnName("max_slots_per_loop");
            entity.Property(e => e.MinTimeBetweenSameAd).HasColumnName("min_time_between_same_ad");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.RulesetApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__ruleset__approve__571DF1D5");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RulesetCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ruleset__deleted__5441852A");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.RulesetDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__ruleset__deleted__5629CD9C");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.RulesetUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__ruleset__updated__5535A963");
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__screen__3213E83F5C017789");

            entity.ToTable("screen", "inv");

            entity.HasIndex(e => new { e.TenantId, e.NormalizedName }, "UQ__screen__DA2325AA9DD2430B").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(19, 4)")
                .HasColumnName("amount");
            entity.Property(e => e.ApprovedAt)
                .HasColumnType("datetime")
                .HasColumnName("approved_at");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(40)
                .HasColumnName("concurrency_stamp");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CurrencyCode).HasColumnName("currency_code");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.District).HasColumnName("district");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(9, 6)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(10, 6)")
                .HasColumnName("longitude");
            entity.Property(e => e.MaxDurationSeconds).HasColumnName("max_duration_seconds");
            entity.Property(e => e.MaxFilesizeMb).HasColumnName("max_filesize_mb");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.NormalizedName)
                .HasMaxLength(50)
                .HasColumnName("normalized_name");
            entity.Property(e => e.OrientationType).HasColumnName("orientation_type");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.RulesetId).HasColumnName("ruleset_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Subcategory).HasColumnName("subcategory");
            entity.Property(e => e.SupportedFormats).HasColumnName("supported_formats");
            entity.Property(e => e.SupportsAudio).HasColumnName("supports_audio");
            entity.Property(e => e.SupportsHtml5).HasColumnName("supports_html5");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Tier).HasColumnName("tier");
            entity.Property(e => e.TimeZone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("time_zone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VenueName)
                .HasMaxLength(100)
                .HasColumnName("venue_name");
            entity.Property(e => e.VenueType).HasColumnName("venue_type");
            entity.Property(e => e.Width).HasColumnName("width");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.ScreenApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__screen__approved__6754599E");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ScreenCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen__created___6477ECF3");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ScreenDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__screen__deleted___66603565");

            entity.HasOne(d => d.Player).WithMany(p => p.Screens)
                .HasForeignKey(d => d.PlayerId)
                .HasConstraintName("FK__screen__player_i__628FA481");

            entity.HasOne(d => d.Ruleset).WithMany(p => p.Screens)
                .HasForeignKey(d => d.RulesetId)
                .HasConstraintName("FK__screen__ruleset___6383C8BA");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Screens)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen__tenant_i__619B8048");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ScreenUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__screen__updated___656C112C");
        });

        modelBuilder.Entity<ScreenGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__screen_g__3213E83F2C2E23B9");

            entity.ToTable("screen_group", "inv");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.GroupType).HasColumnName("group_type");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PricingStrategy).HasColumnName("pricing_strategy");
            entity.Property(e => e.ScreenIds).HasColumnName("screen_ids");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ScreenGroupCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen_gr__creat__5AEE82B9");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ScreenGroupDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__screen_gr__delet__5CD6CB2B");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ScreenGroupUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__screen_gr__updat__5BE2A6F2");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tenant__3213E83F7763B694");

            entity.ToTable("tenant", "core");

            entity.HasIndex(e => e.NormalizedName, "UQ__tenant__CD1BA950B87810BF").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(40)
                .HasColumnName("concurrency_stamp");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.NormalizedName)
                .HasMaxLength(50)
                .HasColumnName("normalized_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TenantCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tenant_user_creator");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.TenantDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("fk_tenant_user_del");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TenantUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_tenant_user_upd");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83F6D31EEBE");

            entity.ToTable("user", "core");

            entity.HasIndex(e => new { e.NormalizedEmail, e.TenantId }, "unique_user_per_tenant_email").IsUnique();

            entity.HasIndex(e => new { e.NormalizedUserName, e.TenantId }, "unique_user_per_tenant_user_name").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(40)
                .HasColumnName("concurrency_stamp");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.NormalizedEmail)
                .HasMaxLength(255)
                .HasColumnName("normalized_email");
            entity.Property(e => e.NormalizedUserName)
                .HasMaxLength(255)
                .HasColumnName("normalized_user_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(256)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(16)
                .HasColumnName("phone_number");
            entity.Property(e => e.SecurityStamp)
                .HasMaxLength(256)
                .HasColumnName("security_stamp");
            entity.Property(e => e.SurName)
                .HasMaxLength(50)
                .HasColumnName("sur_name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_creator");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.InverseDeletedByNavigation)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("fk_user_del");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Users)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_tenant");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_user_upd");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
