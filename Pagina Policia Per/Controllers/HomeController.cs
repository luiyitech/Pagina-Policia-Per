using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System.Diagnostics;
using System.Linq;
using Pagina_Policia_Per.Data; // <--- Aseg�rate de tener esta directiva using para ApplicationDbContext
using Microsoft.EntityFrameworkCore; // <--- Necesario para usar m�todos de extensi�n de EF Core como ToListAsync()
using System.Threading.Tasks; // <--- Necesario para usar async/await

namespace Pagina_Policia_Per.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // <--- Agregamos una variable para el contexto de base de datos

        // Modificamos el constructor para inyectar ILogger Y ApplicationDbContext
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // <--- Asignamos el contexto inyectado
        }

        // --- Modificamos la acci�n Index para que sea as�ncrona y lea del DbContext ---
        public async Task<IActionResult> Index()
        {
            // 1. Leemos las noticias desde el DbContext
            // 2. Las ordenamos de la m�s nueva a la m�s vieja
            // 3. Tomamos solo las primeras 3
            // 4. Usamos ToListAsync() para obtener los resultados de forma as�ncrona
            var ultimasNoticias = await _context.Noticia // <--- Leemos de _context.Noticia
                .OrderByDescending(n => n.FechaPublicacion)
                .Take(3)
                .ToListAsync(); // <--- Usamos ToListAsync()

            // 5. Le pasamos esa lista de 3 noticias a la vista
            return View(ultimasNoticias);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
