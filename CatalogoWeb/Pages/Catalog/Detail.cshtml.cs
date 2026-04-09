using CatalogoWeb.Data;
using CatalogoWeb.Data.Ventas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatalogoWeb.Pages.Catalog;

public class DetailModel : PageModel
{
    private readonly VentasDbContext _ventas;

    public DetailModel(VentasDbContext ventas)
    {
        _ventas = ventas;
    }

    public VentasProduct? Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Product = await _ventas.Products
            .AsNoTracking()
            .Include(p => p.ProductGroup)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (Product == null)
            return NotFound();

        ViewData["Title"] = Product.Name;
        return Page();
    }
}
