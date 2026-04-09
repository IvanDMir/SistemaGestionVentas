namespace SistemaGestionVentas.Models;

/// <summary>
/// Agrupa variantes del mismo tipo de artículo (ej. familia "Rompecabezas").
/// </summary>
public class ProductGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
