using CatalogoWeb.Data;
using CatalogoWeb.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CatalogoWeb.Infrastructure;

public static class CatalogDbInitializer
{
    public static async Task InitializeAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(CatalogDbInitializer));

        var env = services.GetRequiredService<IWebHostEnvironment>();
        var uploads = Path.Combine(env.WebRootPath, "uploads", "catalog");
        Directory.CreateDirectory(uploads);

        var context = services.GetRequiredService<CatalogDbContext>();
        await context.Database.MigrateAsync();

        var adminOpts = services.GetRequiredService<IOptions<AdminSeedOptions>>().Value;
        if (string.IsNullOrWhiteSpace(adminOpts.Password))
        {
            logger.LogWarning(
                "Admin:Password no está configurado. Definilo en User Secrets o appsettings (solo desarrollo). No se creó usuario admin.");
            return;
        }

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var existing = await userManager.FindByNameAsync(adminOpts.UserName);
        if (existing != null)
            return;

        var user = new ApplicationUser
        {
            UserName = adminOpts.UserName,
            Email = adminOpts.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminOpts.Password);
        if (!result.Succeeded)
            logger.LogError("No se pudo crear el usuario admin: {Errors}",
                string.Join("; ", result.Errors.Select(e => e.Description)));
    }
}
