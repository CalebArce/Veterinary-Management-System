namespace VeterinarySystem.Infrastructure.Data;

using global::VeterinarySystem.Domain.Entities;
using global::VeterinarySystem.Domain.Enums;
using VeterinarySystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

/// <summary>
/// Main Entity Framework database context used for application persistence
/// </summary>
public class AppDbContext : DbContext
{
    private readonly ICurrentUserService? currentUserService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService? _currentUserService = null) : base(options)
    {
        currentUserService = _currentUserService;
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<TypeService> TypeServices => Set<TypeService>();
    public DbSet<PetService> PetServices => Set<PetService>();
    public DbSet<User> Users => Set<User>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureClient(modelBuilder);
        ConfigurePet(modelBuilder);
        ConfigureTypeService(modelBuilder);
        ConfigurePetService(modelBuilder);
        ConfigureUser(modelBuilder);
        ConfigureAuditLog(modelBuilder);
        ConfigureRefreshToken(modelBuilder);
    }
    
    private static void ConfigureClient(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Clients");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Identification)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(c => c.Identification)
                .IsUnique();

            entity.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(c => c.Email)
                .HasMaxLength(150);

            entity.Property(c => c.Address)
                .HasMaxLength(250);

            entity.Property(c => c.IsActive)
                .HasDefaultValue(true);

            entity.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(c => c.CreatedBy)
                .HasMaxLength(150);

            entity.Property(c => c.UpdatedBy)
                .HasMaxLength(150);
            // Business Rule:
            // A pet can only belong to one client
            entity.HasMany(c => c.Pets)
                .WithOne(p => p.client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigurePet(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>(entity =>
        {
            entity.ToTable("Pets");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Species)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Breed)
                .HasMaxLength(100);

            entity.Property(p => p.Weight)
                .HasPrecision(10, 2);

            entity.Property(p => p.IsActive)
                .HasDefaultValue(true);

            entity.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(c => c.CreatedBy)
                .HasMaxLength(150);

            entity.Property(c => c.UpdatedBy)
                .HasMaxLength(150);
            entity.HasMany(p => p.petServices)
                .WithOne(ps => ps.pet)
                .HasForeignKey(ps => ps.PetId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureTypeService(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TypeService>(entity =>
        {
            entity.ToTable("TypeServices");

            entity.HasKey(ts => ts.Id);

            entity.Property(ts => ts.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(ts => ts.Description)
                .HasMaxLength(250);

            entity.Property(ts => ts.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(ts => ts.DurationMinutes)
                .IsRequired();

            entity.Property(ts => ts.IsActive)
                .HasDefaultValue(true);
            entity.Property(c => c.CreatedBy)
                .HasMaxLength(150);

            entity.Property(c => c.UpdatedBy)
                .HasMaxLength(150);
            entity.Property(ts => ts.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });
    }

    private static void ConfigurePetService(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PetService>(entity =>
        {
            entity.ToTable("PetServices");

            entity.HasKey(ps => ps.Id);

            entity.Property(ps => ps.FinalPrice)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(ps => ps.DurationMinutes)
                .IsRequired();

            entity.Property(ps => ps.billingStatus)
                .HasConversion<int>()
                .HasDefaultValue(BillingStatus.Pending);

            entity.Property(ps => ps.Notes)
                .HasMaxLength(500);

            entity.Property(ps => ps.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            entity.Property(c => c.CreatedBy)
                .HasMaxLength(150);

            entity.Property(c => c.UpdatedBy)
                .HasMaxLength(150);
            entity.HasOne(ps => ps.pet)
                .WithMany(p => p.petServices)
                .HasForeignKey(ps => ps.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ps => ps.typeService)
                .WithMany(ts => ts.petServices)
                .HasForeignKey(ps => ps.TypeServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void SeedTypeServices (ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TypeService>().HasData(
            new TypeService
            {
                Id = 1,
                Name = "Consulta general",
                Description = "Consulta veterinaria básica.",
                Price = 15000,
                DurationMinutes = 30,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new TypeService
            {
                Id = 2,
                Name = "Vacunación anual",
                Description = "Aplicación de vacunas anuales.",
                Price = 40000,
                DurationMinutes = 20,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new TypeService
            {
                Id = 3,
                Name = "Grooming pequeño",
                Description = "Servicio de grooming para mascota pequeña.",
                Price = 15000,
                DurationMinutes = 60,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Role)
                .HasConversion<int>();

            entity.Property(u => u.IsActive)
                .HasDefaultValue(true);

            entity.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
            
            entity.Property(c => c.CreatedBy)
                .HasMaxLength(150);

            entity.Property(c => c.UpdatedBy)
                .HasMaxLength(150);
        });
    }

    private void ApplyAuditInfo()
    {
        var entries = ChangeTracker
            .Entries<AuditableEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        var currentUser = currentUserService?.Email ?? "System";

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = currentUser;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedBy = currentUser;

                entry.Property(x => x.CreatedAt).IsModified = false;
                entry.Property(x => x.CreatedBy).IsModified = false;
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private static void ConfigureAuditLog(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Action)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(a => a.EntityName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.Description)
                .HasMaxLength(500);

            entity.Property(a => a.UserEmail)
                .HasMaxLength(150);

            entity.Property(a => a.UserRole)
                .HasMaxLength(50);

            entity.Property(a => a.IpAddress)
                .HasMaxLength(80);

            entity.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });
    }

    private static void ConfigureRefreshToken(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");

            entity.HasKey(rt => rt.Id);

            entity.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(rt => rt.ExpiresAt)
                .IsRequired();

            entity.Property(rt => rt.IsRevoked)
                .HasDefaultValue(false);

            entity.Property(rt => rt.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.Property(rt => rt.RevokedAt)
                .IsRequired(false);

            entity.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    
}