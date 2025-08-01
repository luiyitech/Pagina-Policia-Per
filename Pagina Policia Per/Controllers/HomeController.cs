using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models; // ¡Importante para que reconozca el modelo Noticia!
using System.Diagnostics;
using System.Linq; // ¡Importante para poder usar OrderByDescending y Take!

namespace Pagina_Policia_Per.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // 1. Accedemos a la lista pública de noticias del NoticiasController
            // 2. Las ordenamos de la más nueva a la más vieja
            // 3. Tomamos solo las primeras 3
            var ultimasNoticias = NoticiasController._allNoticias
                .OrderByDescending(n => n.FechaPublicacion)
                .Take(3)
                .ToList();

            // 4. Le pasamos esa lista de 3 noticias a la vista
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