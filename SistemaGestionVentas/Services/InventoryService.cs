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
    /// <paramref name="unitPrice"/>: en <see cref="MovementType.Venta"/> es precio de venta unitario;
    /// en <see cref="MovementType.Entrada"/> es el costo unitario de esa compra (actualiza el promedio ponderado).
    /// </summary>
    public void RegisterMovement(int productId, MovementType type, int quantity, decimal unitPrice, string? note)
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

            decimal unitCostSnapshot;
            decimal unitSalePrice = 0m;

            switch (type)
            {
                case MovementType.Entrada:
                    if (unitPrice < 0)
                        throw new ArgumentException("El costo unitario de la compra no puede ser negativo.");
                    var sIn = product.StockQuantity;
                    var pAvg = product.CostPrice;
                    var qIn = quantity;
                    product.CostPrice = sIn <= 0
                        ? unitPrice
                        : (sIn * pAvg + qIn * unitPrice) / (sIn + qIn);
                    unitCostSnapshot = unitPrice;
                    break;

                case MovementType.Venta:
                    if (unitPrice < 0)
                        throw new ArgumentException("El precio de venta no puede ser negativo.");
                    unitSalePrice = unitPrice;
                    unitCostSnapshot = product.CostPrice;
                    break;

                case MovementType.AjustePositivo:
                    unitCostSnapshot = product.CostPrice;
                    break;

                default:
                    unitCostSnapshot = product.CostPrice;
                    break;
            }

            var movement = new StockMovement
            {
                ProductId = productId,
                Type = type,
                Quantity = quantity,
                UnitSalePrice = unitSalePrice,
                UnitCostSnapshot = unitCostSnapshot,
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
