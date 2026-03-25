namespace SistemaGestionVentas.Models;

public class StockMovement
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public MovementType Type { get; set; }

    public int Quantity { get; set; }

    /// <summary>Precio unitario de venta real (solo relevante para Venta).</summary>
    public decimal UnitSalePrice { get; set; }

    /// <summary>Costo unitario del producto al momento del movimiento.</summary>
    public decimal UnitCostSnapshot { get; set; }

    public DateTime OccurredAt { get; set; }

    public string? Note { get; set; }
}
