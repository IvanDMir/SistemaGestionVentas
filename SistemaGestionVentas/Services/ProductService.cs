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

    public List<Product> GetAll()
    {
        using var ctx = new AppDbContext(_options);
        return ctx.Products.AsNoTracking().OrderBy(p => p.Name).ToList();
    }

    public Product? GetById(int id)
    {
        using var ctx = new AppDbContext(_options);
        return ctx.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
    }

    public void Create(string name, string? sku, decimal costPrice, decimal listSalePrice, int initialStock, int minStockThreshold)
    {
        Validate(name, costPrice, listSalePrice, minStockThreshold);
        if (initialStock < 0)
            throw new ArgumentException("El stock inicial no puede ser negativo.", nameof(initialStock));

        using var ctx = new AppDbContext(_options);
        ctx.Products.Add(new Product
        {
            Name = name.Trim(),
            Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim(),
            CostPrice = costPrice,
            ListSalePrice = listSalePrice,
            StockQuantity = initialStock,
            MinStockThreshold = minStockThreshold
        });
        ctx.SaveChanges();
    }

    public void Update(int id, string name, string? sku, decimal costPrice, decimal listSalePrice, int minStockThreshold)
    {
        Validate(name, costPrice, listSalePrice, minStockThreshold);

        using var ctx = new AppDbContext(_options);
        var product = ctx.Products.FirstOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException("Producto no encontrado.");

        product.Name = name.Trim();
        product.Sku = string.IsNullOrWhiteSpace(sku) ? null : sku.Trim();
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
}
