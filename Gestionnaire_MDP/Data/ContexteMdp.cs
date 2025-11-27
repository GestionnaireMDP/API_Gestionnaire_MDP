using Gestionnaire_MDP.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Gestionnaire_MDP.Data;

public class ContexteMdp : DbContext
{
    public ContexteMdp(DbContextOptions<ContexteMdp> options) : base(options)
    {
    }
    
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Vault> Vaults { get; set; }
    public virtual DbSet<VaultEntry> VaultEntries { get; set; }

    // Surcharge de SaveChangesAsync pour gérer les timestamps CreatedAtUtc et UpdatedAtUtc
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries()
            .Where(e =>
                (e.Entity is User || e.Entity is Vault || e.Entity is VaultEntry) &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

        DateTime now = DateTime.UtcNow;

        foreach (EntityEntry entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((dynamic)entry.Entity).CreatedAtUtc = now;
                ((dynamic)entry.Entity).UpdatedAtUtc = null;
            }
            else if (entry.State == EntityState.Modified)
                ((dynamic)entry.Entity).UpdatedAtUtc = now;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    
    // Permet de configurer les entités et leurs relations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.EntraId)
                .HasMaxLength(256)
                .IsRequired();
            entity.Property(u => u.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Vault>(entity =>
        {
            entity.ToTable("Vaults");
            entity.HasKey(v => v.Id);
            
            entity.Property(v => v.HashMdp)
                .HasMaxLength(256)
                .IsRequired();
            
            entity.Property(v => v.Argon2Salt)
                .IsRequired();

            entity.Property(v => v.Argon2MemoryKb)
                .IsRequired();

            entity.Property(v => v.Argon2Iterations)
                .IsRequired();

            entity.Property(v => v.Argon2Parallelism)
                .IsRequired();

            entity.Property(v => v.Argon2HashLength)
                .IsRequired();

            entity.Property(v => v.Argon2Version)
                .IsRequired();
            
            entity.HasOne(v => v.User)
                .WithOne(u => u.Vault)
                .HasForeignKey<Vault>(v => v.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<VaultEntry>(entity =>
        {
            entity.ToTable("VaultEntries");
            entity.HasKey(ve => ve.Id);
            
            entity.Property(ve => ve.EncryptedData)
                .IsRequired();
            
            entity.Property(ve => ve.EncryptionIv)
                .IsRequired();
            
            entity.Property(ve => ve.EncryptionTag)
                .IsRequired();
            
            entity.HasOne(ve => ve.Vault)
                .WithMany(v => v.VaultEntries)
                .HasForeignKey(ve => ve.VaultId)
                .IsRequired();
        });
    }
}