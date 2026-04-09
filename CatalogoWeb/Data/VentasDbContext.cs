using CatalogoWeb.Data.Ventas;
using Microsoft.EntityFrameworkCore;

namespace CatalogoWeb.Data;

/// <summary>Lectura del mismo esquema que el programa de escritorio SistemaGestionVentas (sin migraciones aquí).</summary>
public class VentasDbContext : DbContext
{
    public VentasDbContext(DbContextOptions<VentasDbContext> options)
        : base(options)
    {
    }

    public DbSet<VentasProduct> Products => Set<VentasProduct>();
    public DbSet<VentasProductGroup> ProductGroups => Set<VentasProductGroup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VentasProductGroup>(entity =>
        {
            entity.Property(g => g.Name).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<VentasProduct>(entity =>
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
    }
}
