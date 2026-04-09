using CatalogoWeb.Data;
using CatalogoWeb.Infrastructure;
using CatalogoWeb.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection(SiteOptions.SectionName));
builder.Services.Configure<AdminSeedOptions>(builder.Configuration.GetSection(AdminSeedOptions.SectionName));
builder.Services.Configure<VentasDatabaseOptions>(builder.Configuration.GetSection(VentasDatabaseOptions.SectionName));

var useSqlite = builder.Configuration.GetValue<bool?>("Database:UseSqlite") ?? true;

string sharedConnectionString;
if (useSqlite)
{
    var ventasOpts = builder.Configuration.GetSection(VentasDatabaseOptions.SectionName).Get<VentasDatabaseOptions>()
                     ?? new VentasDatabaseOptions();
    var ventasPath = ventasOpts.DatabasePath;
    if (string.IsNullOrWhiteSpace(ventasPath))
    {
        ventasPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SistemaGestionVentas",
            "ventas.db");
    }

    var dir = Path.GetDirectoryName(Path.GetFullPath(ventasPath));
    if (!string.IsNullOrEmpty(dir))
        Directory.CreateDirectory(dir);

    sharedConnectionString = $"Data Source={ventasPath}";
}
else
{
    sharedConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                             ?? throw new InvalidOperationException(
                                 "Definí ConnectionStrings:DefaultConnection cuando Database:UseSqlite es false.");
}

builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    if (useSqlite)
        options.UseSqlite(sharedConnectionString);
    else
        options.UseSqlServer(sharedConnectionString);
});

builder.Services.AddDbContext<VentasDbContext>(options =>
{
    if (useSqlite)
        options.UseSqlite(sharedConnectionString);
    else
        options.UseSqlServer(sharedConnectionString);
});

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<CatalogDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Login";
    options.LogoutPath = "/Admin/Logout";
    options.AccessDeniedPath = "/Admin/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin");
    options.Conventions.AllowAnonymousToPage("/Admin/Login");
});

var app = builder.Build();

await CatalogDbInitializer.InitializeAsync(app);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
