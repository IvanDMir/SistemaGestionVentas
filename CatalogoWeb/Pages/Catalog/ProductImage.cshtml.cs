using CatalogoWeb.Data;
using CatalogoWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatalogoWeb.Pages.Catalog;

public class ProductImageModel : PageModel
{
    private readonly VentasDbContext _ventas;

    public ProductImageModel(VentasDbContext ventas)
    {
        _ventas = ventas;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var row = await _ventas.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new { p.ImageData })
            .FirstOrDefaultAsync(cancellationToken);

        if (row?.ImageData is not { Length: > 0 })
            return NotFound();

        var contentType = ImageFormatSniffer.GetContentType(row.ImageData.AsSpan()) ?? "application/octet-stream";
        return File(row.ImageData, contentType);
    }
}
