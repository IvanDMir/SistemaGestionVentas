namespace SistemaGestionVentas.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Sku { get; set; }

    public decimal CostPrice { get; set; }

    public decimal ListSalePrice { get; set; }

    public int StockQuantity { get; set; }

    public int MinStockThreshold { get; set; }

    public ICollection<StockMovement> Movements { get; set; } = new List<StockMovement>();
}
