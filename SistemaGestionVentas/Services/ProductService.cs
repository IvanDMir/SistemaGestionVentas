using Microsoft.EntityFrameworkCore;
using SistemaGestionVentas.Data;
using SistemaGestionVentas.Models;

namespace SistemaGestionVentas.Services;

public class ProductService
{
    private readonly DbContextOptions<AppDbContext> _options;

    public ProductService(DbContextOptions<AppDbContext> options)
    {
        _options = options;
    }

    public List<ProductGroup> GetAllGroups()
    {
        using var ctx = new AppDbContext(_options);
        return ctx.ProductGroups.AsNoTracking().OrderBy(g => g.Name).ToList();
    }

    public List<Product> GetAll()
    {
        using var ctx = new AppDbContext(_options);
        return ctx.Products.AsNoTracking()
            .Include(p => p.ProductGroup)
            .OrderBy(p => p.ProductGroup != null ? p.ProductGroup.Name : "")
            .ThenBy(p => p.Name)
            .ThenBy(p => p.VariantLabel ?? "")
            .ToList();
    }

    public Product? GetById(int id)
    {
        using var ctx = new AppDbContext(_options);
        return ctx.Products.AsNoTracking()
            .Include(p => p.ProductGroup)
            .FirstOrDefault(p => p.Id == id);
    }

    /// <summary>
    /// Si <paramref name="newGroupName"/> tiene texto, crea o reutiliza un grupo con ese nombre.
    /// Si no, usa <paramref name="selectedGroupId"/> (puede ser null = sin grupo).
    /// </summary>
    public int? ResolveProductGroupId(string? newGroupName, int? selectedGroupId)
    {
        if (!string.IsNullOrWhiteSpace(newGroupName))
        {
            var name = newGroupName.Trim();
            using var ctx = new AppDbContext(_options);
            var list = ctx.ProductGroups.ToList();
            var existing = list.FirstOrDefault(g => string.Equals(g.Name, name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
                return existing.Id;
            var g = new ProductGroup { Name = name };
            ctx.ProductGroups.Add(g);
            ctx.SaveChanges();
            return g.Id;
        }

        return selectedGroupId;
    }

    public void Create(
        int? productGroupId,
        string name,
        string? variantLabel,
        byte[]? imageData,
        decimal costPrice,
        decimal listSalePrice,
        int initialStock,
        int minStockThreshold)
    {
        Validate(name, costPrice, listSalePrice, minStockThreshold);
        if (initialStock < 0)
            throw new ArgumentException("El stock inicial no puede ser negativo.", nameof(initialStock));

        using var ctx = new AppDbContext(_options);
        ctx.Products.Add(new Product
        {
            ProductGroupId = productGroupId,
            Name = name.Trim(),
            VariantLabel = string.IsNullOrWhiteSpace(variantLabel) ? null : variantLabel.Trim(),
            ImageData = CloneBytes(imageData),
            CostPrice = costPrice,
            ListSalePrice = listSalePrice,
            StockQuantity = initialStock,
            MinStockThreshold = minStockThreshold
        });
        ctx.SaveChanges();
    }

    public void Update(
        int id,
        int? productGroupId,
        string name,
        string? variantLabel,
        byte[]? imageData,
        decimal costPrice,
        decimal listSalePrice,
        int minStockThreshold)
    {
        Validate(name, costPrice, listSalePrice, minStockThreshold);

        using var ctx = new AppDbContext(_options);
        var product = ctx.Products.FirstOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException("Producto no encontrado.");

        product.ProductGroupId = productGroupId;
        product.Name = name.Trim();
        product.VariantLabel = string.IsNullOrWhiteSpace(variantLabel) ? null : variantLabel.Trim();
        product.ImageData = CloneBytes(imageData);
        product.CostPrice = costPrice;
        product.ListSalePrice = listSalePrice;
        product.MinStockThreshold = minStockThreshold;
        ctx.SaveChanges();
    }

    public void Delete(int id)
    {
        using var ctx = new AppDbContext(_options);
        var hasMovements = ctx.StockMovements.Any(m => m.ProductId == id);
        if (hasMovements)
            throw new InvalidOperationException("No se puede eliminar: el producto tiene movimientos de stock registrados.");

        var product = ctx.Products.FirstOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException("Producto no encontrado.");

        ctx.Products.Remove(product);
        ctx.SaveChanges();
    }

    private static void Validate(string name, decimal costPrice, decimal listSalePrice, int minStockThreshold)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es obligatorio.", nameof(name));
        if (costPrice < 0 || listSalePrice < 0)
            throw new ArgumentException("Los precios no pueden ser negativos.");
        if (minStockThreshold < 0)
            throw new ArgumentException("El umbral mínimo no puede ser negativo.");
    }

    private static byte[]? CloneBytes(byte[]? data) =>
        data == null ? null : (byte[])data.Clone();
}
