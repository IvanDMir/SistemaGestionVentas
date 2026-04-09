using CatalogoWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatalogoWeb.Pages.Admin.Suggestions;

[Authorize]
public class IndexModel : PageModel
{
    private readonly CatalogDbContext _db;

    public IndexModel(CatalogDbContext db)
    {
        _db = db;
    }

    public IList<SuggestionRow> Items { get; set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Items = await _db.CatalogSuggestions
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new SuggestionRow(s.Id, s.Message, s.MercadoLibreUrl, s.Contact, s.CreatedAt))
            .ToListAsync(cancellationToken);
    }

    public record SuggestionRow(int Id, string Message, string MercadoLibreUrl, string? Contact, DateTimeOffset CreatedAt);
}
