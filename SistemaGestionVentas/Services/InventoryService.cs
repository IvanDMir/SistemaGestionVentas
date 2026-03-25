using Microsoft.EntityFrameworkCore;
using SistemaGestionVentas.Data;
using SistemaGestionVentas.Models;

namespace SistemaGestionVentas.Services;

public class InventoryService
{
    private readonly DbContextOptions<AppDbContext> _options;

    public InventoryService(DbContextOptions<AppDbContext> options)
    {
        _options = options;
    }

    /// <summary>
    /// Registra un movimiento y actualiza el stock del producto en una sola transacción.
    /// </summary>
    public void RegisterMovement(int productId, MovementType type, int quantity, decimal unitSalePrice, string? note)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.", nameof(quantity));

        using var ctx = new AppDbContext(_options);
        using var tx = ctx.Database.BeginTransaction();
        try
        {
            var product = ctx.Products.FirstOrDefault(p => p.Id == productId)
                ?? throw new InvalidOperationException("Producto no encontrado.");

            var delta = GetStockDelta(type, quantity);
            var newStock = product.StockQuantity + delta;
            if (newStock < 0)
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {product.StockQuantity}, se necesitan: {quantity}.");

            var costSnapshot = product.CostPrice;
            var salePrice = type == MovementType.Venta ? unitSalePrice : 0m;
            if (type == MovementType.Venta && salePrice < 0)
                throw new ArgumentException("El precio de venta no puede ser negativo.");

            var movement = new StockMovement
            {
                ProductId = productId,
                Type = type,
                Quantity = quantity,
                UnitSalePrice = salePrice,
                UnitCostSnapshot = costSnapshot,
                OccurredAt = DateTime.Now,
                Note = string.IsNullOrWhiteSpace(note) ? null : note.Trim()
            };

            product.StockQuantity = newStock;
            ctx.StockMovements.Add(movement);
            ctx.SaveChanges();
            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    private static int GetStockDelta(MovementType type, int quantity) => type switch
    {
        MovementType.Entrada or MovementType.AjustePositivo => quantity,
        MovementType.Salida or MovementType.Venta or MovementType.AjusteNegativo => -quantity,
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };
}
