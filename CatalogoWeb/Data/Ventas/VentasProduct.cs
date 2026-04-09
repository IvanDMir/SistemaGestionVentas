using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogoWeb.Data.Ventas;

[Table("Products")]
public class VentasProduct
{
    public int Id { get; set; }

    public int? ProductGroupId { get; set; }

    public VentasProductGroup? ProductGroup { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    [MaxLength(200)]
    public string? VariantLabel { get; set; }

    public byte[]? ImageData { get; set; }

    public decimal CostPrice { get; set; }

    public decimal ListSalePrice { get; set; }

    public int StockQuantity { get; set; }

    public int MinStockThreshold { get; set; }
}
