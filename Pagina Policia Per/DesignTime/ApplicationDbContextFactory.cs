using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Pagina_Policia_Per.Data // O el namespace donde creaste ApplicationDbContextFactory
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Obtiene la configuración de la aplicación (incluida la cadena de conexión)
            // Esto simula cómo se carga la configuración en una aplicación real
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Establece la ruta base al directorio actual
                .AddJsonFile("appsettings.json") // Carga appsettings.json
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true) // Carga appsettings.Environment.json
                .Build();

            // Configura las opciones del DbContext para usar SQL Server
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Obtiene la cadena de conexión desde la configuración
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configura el DbContext para usar SQL Server con la cadena de conexión
            builder.UseSqlServer(connectionString);

            // Crea y devuelve una instancia del DbContext con las opciones configuradas
            return new ApplicationDbContext(builder.Options);
        }
    }
}
