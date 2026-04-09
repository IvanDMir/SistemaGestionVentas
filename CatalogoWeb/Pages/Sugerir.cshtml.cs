using System.ComponentModel.DataAnnotations;
using CatalogoWeb.Data;
using CatalogoWeb.Models;
using CatalogoWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatalogoWeb.Pages;

public class SugerirModel : PageModel
{
    private readonly CatalogDbContext _db;

    public SugerirModel(CatalogDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public bool Sent { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Contanos qué te gustaría que vendamos.")]
        [Display(Name = "Tu idea o recomendación")]
        public string Message { get; set; } = "";

        [Required(ErrorMessage = "Pegá el link de Mercado Libre.")]
        [MaxLength(2000)]
        [Display(Name = "Link de Mercado Libre (publicación de ejemplo)")]
        public string MercadoLibreUrl { get; set; } = "";

        [MaxLength(200)]
        [Display(Name = "Tu nombre o contacto (opcional)")]
        public string? Contact { get; set; }
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return Page();

        if (!MercadoLibreUrlValidator.IsValidMercadoLibreUrl(Input.MercadoLibreUrl))
        {
            ModelState.AddModelError(nameof(Input.MercadoLibreUrl),
                "El link debe ser de Mercado Libre (mercadolibre.com, mercadolivre.com, etc.).");
            return Page();
        }

        _db.CatalogSuggestions.Add(new CatalogSuggestion
        {
            Message = Input.Message.Trim(),
            MercadoLibreUrl = Input.MercadoLibreUrl.Trim(),
            Contact = string.IsNullOrWhiteSpace(Input.Contact) ? null : Input.Contact.Trim(),
            CreatedAt = DateTimeOffset.UtcNow
        });

        await _db.SaveChangesAsync(cancellationToken);
        Sent = true;
        Input = new InputModel();
        return Page();
    }
}
