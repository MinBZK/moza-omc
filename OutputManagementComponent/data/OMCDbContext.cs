using Microsoft.EntityFrameworkCore;

using OutputManagementComponent.data.Entities;
using OutputManagementComponent.Data.Entities;

namespace OutputManagementComponent.data;

public class OMCDbContext(DbContextOptions<OMCDbContext> options) : DbContext(options)
{
    public DbSet<NotificatieEntity> notificaties { get; set; }
    public DbSet<OndernemingEntity> ondernemingen { get; set; }
    public DbSet<NotificatieEventEntity> notificatieevents { get; set; }
    public DbSet<ContactMethodeEntity> contactmethodes { get; set; }

    public override int SaveChanges()
    {
        this.UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = this.ChangeTracker.Entries()
            .Where(e => e.Entity is AuditableEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (AuditableEntity)entry.Entity;

            if (entry.State == EntityState.Added)
                entity.DateCreated = DateTime.UtcNow;

            entity.DateLastUpdated = DateTime.UtcNow;
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NotificatieEntity>()
            .HasIndex(n => n.Reference)
            .IsUnique();

        modelBuilder.Entity<NotificatieEntity>()
            .HasAlternateKey(n => n.Reference);

        modelBuilder.Entity<NotificatieEntity>()
            .HasOne(n => n.Onderneming)
            .WithMany(o => o.Notificaties)
            .HasForeignKey(n => n.KvkNummer)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NotificatieEventEntity>()
            .HasOne(e => e.Notificatie)
            .WithMany(n => n.Events)
            .HasForeignKey(e => e.Reference)
            .HasPrincipalKey(n => n.Reference)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NotificatieEntity>()
            .HasMany(n => n.ContactMethodes)
            .WithOne(c => c.Notificatie)
            .HasForeignKey(c => c.NotificatieEntityDbId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
