using System.ComponentModel.DataAnnotations;

namespace CatalogoWeb.Models;

public class CatalogSuggestion
{
    public int Id { get; set; }

    [Required]
    public string Message { get; set; } = "";

    [Required]
    [MaxLength(2000)]
    public string MercadoLibreUrl { get; set; } = "";

    [MaxLength(200)]
    public string? Contact { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
