namespace CatalogoWeb.Options;

public class AdminSeedOptions
{
    public const string SectionName = "Admin";

    public string UserName { get; set; } = "admin";
    public string Email { get; set; } = "admin@local";
    public string Password { get; set; } = "";
}
