namespace CatalogoWeb.Options;

/// <summary>Ruta al SQLite de SistemaGestionVentas (ventas.db). Vacío = %LocalAppData%\SistemaGestionVentas\ventas.db</summary>
public class VentasDatabaseOptions
{
    public const string SectionName = "Ventas";

    /// <summary>Ruta absoluta al archivo .db, o vacío para la misma carpeta que usa el programa de escritorio.</summary>
    public string? DatabasePath { get; set; }

    /// <summary>Si es true, solo se listan productos con stock &gt; 0. Si es false, se muestran todos.</summary>
    public bool OnlyInStock { get; set; } = false;
}
