namespace SistemaGestionVentas.Models;

public class Product
{
    public int Id { get; set; }

    /// <summary>Línea o familia opcional (ej. Rompecabezas).</summary>
    public int? ProductGroupId { get; set; }

    public ProductGroup? ProductGroup { get; set; }

    /// <summary>Nombre del artículo dentro de la línea (ej. Paisaje montaña).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Modelo o variante (ej. 1000 piezas, edición 2024).</summary>
    public string? VariantLabel { get; set; }

    /// <summary>Imagen del producto (PNG, JPEG, etc.) guardada en la base de datos.</summary>
    public byte[]? ImageData { get; set; }

    /// <summary>
    /// Costo promedio ponderado del stock actual. Se actualiza con cada entrada de compra;
    /// sirve de referencia y para ventas hasta la próxima compra.
    /// </summary>
    public decimal CostPrice { get; set; }

    public decimal ListSalePrice { get; set; }

    public int StockQuantity { get; set; }

    public int MinStockThreshold { get; set; }

    public ICollection<StockMovement> Movements { get; set; } = new List<StockMovement>();
}
