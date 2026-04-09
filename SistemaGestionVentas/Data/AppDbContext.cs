using Microsoft.EntityFrameworkCore;
using SistemaGestionVentas.Models;

namespace SistemaGestionVentas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductGroup>(entity =>
        {
            entity.Property(g => g.Name).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
            entity.Property(p => p.VariantLabel).HasMaxLength(200);
            entity.Property(p => p.CostPrice).HasPrecision(18, 2);
            entity.Property(p => p.ListSalePrice).HasPrecision(18, 2);
            entity.HasOne(p => p.ProductGroup)
                .WithMany(g => g.Products)
                .HasForeignKey(p => p.ProductGroupId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.Property(m => m.UnitSalePrice).HasPrecision(18, 2);
            entity.Property(m => m.UnitCostSnapshot).HasPrecision(18, 2);
            entity.Property(m => m.Note).HasMaxLength(500);
            entity.HasOne(m => m.Product)
                .WithMany(p => p.Movements)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
