using CatalogoWeb.Data;
using CatalogoWeb.Data.Ventas;
using CatalogoWeb.Options;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CatalogoWeb.Pages.Catalog;

public class IndexModel : PageModel
{
    private readonly VentasDbContext _ventas;
    private readonly VentasDatabaseOptions _ventasOpts;

    public IndexModel(VentasDbContext ventas, IOptions<VentasDatabaseOptions> ventasOpts)
    {
        _ventas = ventas;
        _ventasOpts = ventasOpts.Value;
    }

    public string? Search { get; set; }

    public IList<VentasProduct> Items { get; set; } = [];

    public async Task OnGetAsync(string? q, CancellationToken cancellationToken)
    {
        Search = q;

        IQueryable<VentasProduct> query = _ventas.Products
            .AsNoTracking()
            .Include(p => p.ProductGroup);

        if (_ventasOpts.OnlyInStock)
            query = query.Where(p => p.StockQuantity > 0);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(p =>
                p.Name.Contains(term)
                || (p.VariantLabel != null && p.VariantLabel.Contains(term))
                || (p.ProductGroup != null && p.ProductGroup.Name.Contains(term)));
        }

        Items = await query
            .OrderBy(p => p.ProductGroup != null ? p.ProductGroup.Name : "")
            .ThenBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}
