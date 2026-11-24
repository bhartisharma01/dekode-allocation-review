using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace backend.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<AllocationRun> AllocationRuns => Set<AllocationRun>();

    public DbSet<AllocationGap> AllocationGaps => Set<AllocationGap>();

    public DbSet<BatchAllocation> BatchAllocations =>
        Set<BatchAllocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AllocationRun>(entity =>
        {
            entity.Property(x => x.RunIdentifier)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(x => x.RunIdentifier)
                .IsUnique();
        });

        modelBuilder.Entity<AllocationGap>(entity =>
        {
            entity.Property(x => x.GapIdentifier)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Store)
                .HasMaxLength(100);

            entity.Property(x => x.Brand)
                .HasMaxLength(100);

            entity.Property(x => x.ProductType)
                .HasMaxLength(100);

            entity.Property(x => x.OriginalShortfall)
                .HasPrecision(18, 2);

            entity.HasIndex(x => new { x.RunId, x.GapIdentifier })
                .IsUnique();
        });

        modelBuilder.Entity<BatchAllocation>(entity =>
        {
            entity.Property(x => x.BatchIdentifier)
                .HasMaxLength(50)
                .IsRequired();
        });
    }
}