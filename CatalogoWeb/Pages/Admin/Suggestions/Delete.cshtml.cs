using CatalogoWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatalogoWeb.Pages.Admin.Suggestions;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly CatalogDbContext _db;

    public DeleteModel(CatalogDbContext db)
    {
        _db = db;
    }

    public IActionResult OnGet()
    {
        return RedirectToPage("./Index");
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        var row = await _db.CatalogSuggestions.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        if (row == null)
            return NotFound();

        _db.CatalogSuggestions.Remove(row);
        await _db.SaveChangesAsync(cancellationToken);
        return RedirectToPage("./Index");
    }
}
