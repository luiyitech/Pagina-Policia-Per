using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pagina_Policia_Per.Models; // Asegúrate de tener esta directiva using para tu modelo Noticia

namespace Pagina_Policia_Per.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ===================================================================
        //           AGREGAMOS ESTE DbSet para tu modelo Noticia
        // ===================================================================
        public DbSet<Noticia> Noticia { get; set; } // <-- AGREGAMOS ESTA LÍNEA
        // ===================================================================

        // Si en el futuro agregas más modelos de aplicación, también agregarías un DbSet para cada uno aquí.
        // Por ejemplo:
        // public DbSet<Pagina_Policia_Per.Models.OtroModelo> OtroModelo { get; set; }
    }
}
