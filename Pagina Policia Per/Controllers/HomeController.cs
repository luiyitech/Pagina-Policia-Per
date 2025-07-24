// Archivo: Controllers/HomeController.cs

using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using Pagina_Policia_Per.Services; // Usamos nuestro nuevo servicio
using System.Diagnostics;

namespace Pagina_Policia_Per.Controllers
{
    public class HomeController : Controller
    {
        private readonly NoticiaService _noticiaService;

        public HomeController(NoticiaService noticiaService)
        {
            _noticiaService = noticiaService;
        }

        public IActionResult Index()
        {
            var todasLasNoticias = _noticiaService.GetAllNoticias();
            var noticiasDestacadas = todasLasNoticias.OrderByDescending(n => n.FechaPublicacion).Take(3).ToList();
            return View(noticiasDestacadas);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}