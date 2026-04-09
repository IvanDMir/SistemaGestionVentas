using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogoWeb.Data.Ventas;

[Table("ProductGroups")]
public class VentasProductGroup
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    public ICollection<VentasProduct> Products { get; set; } = new List<VentasProduct>();
}
