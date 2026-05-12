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

    public virtual DbSet<Advertiser> Advertisers { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<CampaignFlight> CampaignFlights { get; set; }

    public virtual DbSet<CampaignFlightScreen> CampaignFlightScreens { get; set; }

    public virtual DbSet<CampaignPlaylistItem> CampaignPlaylistItems { get; set; }

    public virtual DbSet<CampaignScreenSchedule> CampaignScreenSchedules { get; set; }

    public virtual DbSet<Listitem> Listitems { get; set; }

    public virtual DbSet<ListitemCategory> ListitemCategories { get; set; }

    public virtual DbSet<MediaLibrary> MediaLibraries { get; set; }

    public virtual DbSet<Screen> Screens { get; set; }

    public virtual DbSet<ScreenOperatingHour> ScreenOperatingHours { get; set; }

    public virtual DbSet<ScreenSupportedMedium> ScreenSupportedMedia { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.112,1433;Database=DoohClick;MultipleActiveResultSets=true;user id=gyanju;password=gyanju##;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Advertiser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__advertis__3213E83FE5D3A6AB");

            entity.ToTable("advertiser", "crm");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(60)
                .HasColumnName("contact_email");
            entity.Property(e => e.ContactName)
                .HasMaxLength(60)
                .HasColumnName("contact_name");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .HasColumnName("contact_phone");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("uuid");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AdvertiserCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__advertise__creat__02FC7413");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.AdvertiserDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__advertise__delet__04E4BC85");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Advertisers)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__advertise__tenan__00200768");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AdvertiserUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__advertise__updat__03F0984C");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__audit_lo__3213E83F79918793");

            entity.ToTable("audit_log", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(20)
                .HasColumnName("action");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasMaxLength(50)
                .HasColumnName("entity_type");
            entity.Property(e => e.NewValue).HasColumnName("new_value");
            entity.Property(e => e.OldValue).HasColumnName("old_value");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__audit_log__chang__31B762FC");

            entity.HasOne(d => d.Tenant).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__audit_log__tenan__30C33EC3");
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__campaign__3213E83FDC75E978");

            entity.ToTable("campaign");

            entity.HasIndex(e => new { e.TenantId, e.CampaignCode }, "UQ__campaign__1B1708E48C8027AD").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.CampaignCode)
                .HasMaxLength(50)
                .HasColumnName("campaign_code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DurationInDays)
                .HasComputedColumnSql("(datediff(day,[start_date],[end_date]))", true)
                .HasColumnName("duration_in_days");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsLocked).HasColumnName("is_locked");
            entity.Property(e => e.ModifiedAt).HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .HasColumnName("remarks");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("DRAFT")
                .HasColumnName("status");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.Campaigns)
                .HasForeignKey(d => d.AdvertiserId)
                .HasConstraintName("FK__campaign__advert__70A8B9AE");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CampaignCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign__create__73852659");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CampaignDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__campaign__delete__7755B73D");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.CampaignModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK__campaign__modifi__756D6ECB");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Campaigns)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign__tenant__6FB49575");
        });

        modelBuilder.Entity<CampaignFlight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__campaign__3213E83F57820129");

            entity.ToTable("campaign_flight");

            entity.HasIndex(e => new { e.CampaignId, e.StartDate }, "UQ_campaign_flight")
                .IsUnique()
                .HasFilter("([is_deleted]=(0))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Campaign).WithMany(p => p.CampaignFlights)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign___campa__7A3223E8");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CampaignFlights)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__campaign___delet__7C1A6C5A");
        });

        modelBuilder.Entity<CampaignFlightScreen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__campaign__3213E83F3328E7B8");

            entity.ToTable("campaign_flight_screen");

            entity.HasIndex(e => new { e.CampaignFlightId, e.ScreenId }, "UQ_campaign_flight_screen")
                .IsUnique()
                .HasFilter("([is_deleted]=(0))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignFlightId).HasColumnName("campaign_flight_id");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ScreenId).HasColumnName("screen_id");

            entity.HasOne(d => d.CampaignFlight).WithMany(p => p.CampaignFlightScreens)
                .HasForeignKey(d => d.CampaignFlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign___campa__7EF6D905");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CampaignFlightScreens)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__campaign___delet__01D345B0");

            entity.HasOne(d => d.Screen).WithMany(p => p.CampaignFlightScreens)
                .HasForeignKey(d => d.ScreenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign___scree__7FEAFD3E");
        });

        modelBuilder.Entity<CampaignPlaylistItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__campaign__3213E83FAD389C0D");

            entity.ToTable("campaign_playlist_item");

            entity.HasIndex(e => new { e.ScheduleId, e.PlayOrder }, "UQ_playlist_order").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DurationSeconds)
                .HasDefaultValue(30)
                .HasColumnName("duration_seconds");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.MediaId).HasColumnName("media_id");
            entity.Property(e => e.PlayOrder).HasColumnName("play_order");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CampaignPlaylistItemCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign___creat__10216507");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CampaignPlaylistItemDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__campaign___delet__12FDD1B2");

            entity.HasOne(d => d.Media).WithMany(p => p.CampaignPlaylistItems)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign___media__0E391C95");

            entity.HasOne(d => d.Schedule).WithMany(p => p.CampaignPlaylistItems)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign___sched__0D44F85C");
        });

        modelBuilder.Entity<CampaignScreenSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__campaign__3213E83FF7E344AB");

            entity.ToTable("campaign_screen_schedule");

            entity.HasIndex(e => new { e.CampaignFlightScreenId, e.DayOfWeek, e.StartTime }, "UQ_screen_schedule")
                .IsUnique()
                .HasFilter("([is_deleted]=(0))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignFlightScreenId).HasColumnName("campaign_flight_screen_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(50)
                .HasColumnName("day_of_week");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.CampaignFlightScreen).WithMany(p => p.CampaignScreenSchedules)
                .HasForeignKey(d => d.CampaignFlightScreenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign___campa__04AFB25B");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CampaignScreenScheduleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__campaign___creat__05A3D694");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CampaignScreenScheduleDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__campaign___delet__09746778");
        });

        modelBuilder.Entity<Listitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__listitem__3213E83F9A03DCDF");

            entity.ToTable("listitem", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Item)
                .HasMaxLength(100)
                .HasColumnName("item");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Listitems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__listitem__catego__40058253");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Listitems)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK__listitem__tenant__3E1D39E1");
        });

        modelBuilder.Entity<ListitemCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__listitem__3213E83FD38F4E21");

            entity.ToTable("listitem_category", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasColumnName("category");
            entity.Property(e => e.CategoryCode)
                .HasMaxLength(50)
                .HasColumnName("category_code");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.ListitemCategories)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK__listitem___tenan__3B40CD36");
        });

        modelBuilder.Entity<MediaLibrary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__media_li__3213E83FB2C5C1C6");

            entity.ToTable("media_library");

            entity.HasIndex(e => new { e.TenantId, e.DisplayName }, "UQ__media_li__B437EAB85C37AD40").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdvertiserId).HasColumnName("advertiser_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .HasColumnName("display_name");
            entity.Property(e => e.DurationSec)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("duration_sec");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.FileSizeBytes).HasColumnName("file_size_bytes");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(500)
                .HasColumnName("file_url");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsVideo).HasColumnName("is_video");
            entity.Property(e => e.Resolution)
                .HasMaxLength(50)
                .HasColumnName("resolution");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UploadedAt).HasColumnName("uploaded_at");
            entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");
            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("uuid");

            entity.HasOne(d => d.Advertiser).WithMany(p => p.MediaLibraries)
                .HasForeignKey(d => d.AdvertiserId)
                .HasConstraintName("FK__media_lib__adver__6BE40491");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MediaLibraryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__media_lib__creat__1CBC4616");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.MediaLibraryDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__media_lib__delet__1DB06A4F");

            entity.HasOne(d => d.Tenant).WithMany(p => p.MediaLibraries)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__media_lib__tenan__19DFD96B");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.MediaLibraryUploadedByNavigations)
                .HasForeignKey(d => d.UploadedBy)
                .HasConstraintName("FK__media_lib__uploa__1BC821DD");
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__screen__3213E83F6F36739D");

            entity.ToTable("screen", "inv");

            entity.HasIndex(e => new { e.TenantId, e.ScreenCode }, "UQ__screen__0E3F7E16EAC7FF02").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressLine)
                .HasMaxLength(200)
                .HasColumnName("address_line");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .HasColumnName("country_code");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .HasColumnName("currency");
            entity.Property(e => e.DefaultResolution)
                .HasMaxLength(50)
                .HasColumnName("default_resolution");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NormalizedName)
                .HasMaxLength(100)
                .HasColumnName("normalized_name");
            entity.Property(e => e.Orientation)
                .HasMaxLength(50)
                .HasColumnName("orientation");
            entity.Property(e => e.RatePerHour)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("rate_per_hour");
            entity.Property(e => e.ScreenCode)
                .HasMaxLength(40)
                .HasColumnName("screen_code");
            entity.Property(e => e.Tag)
                .HasMaxLength(500)
                .HasColumnName("tag");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Timezone)
                .HasMaxLength(50)
                .HasColumnName("timezone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("uuid");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ScreenCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen__created___72C60C4A");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ScreenDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__screen__deleted___74AE54BC");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Screens)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen__tenant_i__70DDC3D8");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ScreenUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__screen__updated___73BA3083");
        });

        modelBuilder.Entity<ScreenOperatingHour>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__screen_o__3213E83FB389B0BA");

            entity.ToTable("screen_operating_hour", "inv");

            entity.HasIndex(e => new { e.ScreenId, e.DayOfWeek, e.OpenTime }, "UQ_screen_operating_hour_screen_day")
                .IsUnique()
                .HasFilter("([is_deleted]=(0))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AudienceSource)
                .HasMaxLength(50)
                .HasColumnName("audience_source");
            entity.Property(e => e.CloseTime).HasColumnName("close_time");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(50)
                .HasColumnName("day_of_week");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.EstimatedImpression).HasColumnName("estimated_impression");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.OpenTime).HasColumnName("open_time");
            entity.Property(e => e.ScreenId).HasColumnName("screen_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ScreenOperatingHourCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen_op__creat__7C4F7684");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ScreenOperatingHourDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__screen_op__delet__47A6A41B");

            entity.HasOne(d => d.Screen).WithMany(p => p.ScreenOperatingHours)
                .HasForeignKey(d => d.ScreenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen_op__scree__7B5B524B");
        });

        modelBuilder.Entity<ScreenSupportedMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__screen_s__3213E83FD017AE27");

            entity.ToTable("screen_supported_media", "inv");

            entity.HasIndex(e => new { e.ScreenId, e.MediaType }, "UQ_screen_supported_media_screen_media")
                .IsUnique()
                .HasFilter("([is_deleted]=(0))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.MediaType)
                .HasMaxLength(50)
                .HasColumnName("media_type");
            entity.Property(e => e.ScreenId).HasColumnName("screen_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ScreenSupportedMediumCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen_su__creat__787EE5A0");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.ScreenSupportedMediumDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__screen_su__delet__498EEC8D");

            entity.HasOne(d => d.Screen).WithMany(p => p.ScreenSupportedMedia)
                .HasForeignKey(d => d.ScreenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__screen_su__scree__778AC167");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tag__3213E83FFFA11709");

            entity.ToTable("tag", "shared");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TagCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tag__created_by__37703C52");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Tags)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tag__tenant_id__3587F3E0");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TagUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__tag__updated_by__3864608B");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tenant__3213E83F73B971C5");

            entity.ToTable("tenant", "identity");

            entity.HasIndex(e => new { e.Id, e.NormalizedName }, "UQ__tenant__3EC252ABC6158C01").IsUnique();

            entity.HasIndex(e => e.NormalizedName, "UQ__tenant__CD1BA950AC3EA93D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(100)
                .HasColumnName("contact_email");
            entity.Property(e => e.ContactName)
                .HasMaxLength(50)
                .HasColumnName("contact_name");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .HasColumnName("contact_phone");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DefaultCurrency)
                .HasMaxLength(10)
                .HasColumnName("default_currency");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NormalizedName)
                .HasMaxLength(100)
                .HasColumnName("normalized_name");
            entity.Property(e => e.TenantCode)
                .HasMaxLength(40)
                .HasColumnName("tenant_code");
            entity.Property(e => e.TimeZone)
                .HasMaxLength(50)
                .HasColumnName("time_zone");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TenantCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tenant__created___6A30C649");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.TenantDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__tenant__deleted___6C190EBB");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TenantUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__tenant__updated___6B24EA82");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83F3A2B7ECD");

            entity.ToTable("user", "identity");

            entity.HasIndex(e => new { e.TenantId, e.NormalizedUserName }, "UQ__user__0398B716AFF686B9").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.NormalizedEmail)
                .HasMaxLength(100)
                .HasColumnName("normalized_email");
            entity.Property(e => e.NormalizedUserName)
                .HasMaxLength(100)
                .HasColumnName("normalized_user_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(150)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(500)
                .HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenExpiry).HasColumnName("refresh_token_expiry");
            entity.Property(e => e.SurName)
                .HasMaxLength(50)
                .HasColumnName("sur_name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("user_name");
            entity.Property(e => e.UserRole)
                .HasMaxLength(50)
                .HasColumnName("user_role");
            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("uuid");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user__created_by__66603565");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.InverseDeletedByNavigation)
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK__user__deleted_by__68487DD7");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Users)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user__tenant_id__6477ECF3");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__user__updated_by__6754599E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
