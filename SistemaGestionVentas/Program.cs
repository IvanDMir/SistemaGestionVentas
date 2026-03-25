using Microsoft.EntityFrameworkCore;
using SistemaGestionVentas.Data;

namespace SistemaGestionVentas
{
    internal static class Program
    {
        internal static DbContextOptions<AppDbContext> DbContextOptions { get; private set; } = null!;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SistemaGestionVentas");
            Directory.CreateDirectory(folder);
            var dbPath = Path.Combine(folder, "ventas.db");

            DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            using (var db = new AppDbContext(DbContextOptions))
            {
                db.Database.Migrate();
            }

            Application.Run(new Form1());
        }
    }
}
