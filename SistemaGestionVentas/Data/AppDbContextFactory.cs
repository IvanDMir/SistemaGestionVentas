using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaGestionVentas.Data;

/// <summary>
/// Permite que las herramientas de EF (Add-Migration) creen un contexto en tiempo de diseño.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=SistemaGestionVentas_design.db")
            .Options;

        return new AppDbContext(options);
    }
}
