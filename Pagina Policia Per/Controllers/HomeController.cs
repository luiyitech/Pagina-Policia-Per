// Archivo: Controllers/HomeController.cs

using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models; // �A�adimos el using para Noticia!
using System.Diagnostics;

namespace Pagina_Policia_Per.Controllers
{
    public class HomeController : Controller
    {
        // El Logger se usa para registrar eventos internos, lo dejamos como est�.
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // --- �ACCI�N DE INICIO MODIFICADA! ---
        public IActionResult Index()
        {
            // 1. Obtenemos TODAS las noticias
            var todasLasNoticias = _GetNoticias();

            // 2. Las ordenamos por fecha (la m�s nueva primero) y tomamos solo las 3 primeras
            var noticiasDestacadas = todasLasNoticias.OrderByDescending(n => n.FechaPublicacion).Take(3).ToList();

            // 3. Pasamos esa peque�a lista de 3 noticias a la Vista de Inicio
            return View(noticiasDestacadas);
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

        // --- M�todo privado para simular la obtenci�n de datos (lo copiamos aqu�) ---
        private List<Noticia> _GetNoticias()
        {
            // Usamos la misma lista de noticias que en NoticiasController para consistencia
            return new List<Noticia>
            {
                new Noticia { Id = 1, Titulo = "Exitosa Capacitaci�n en Ciberdelitos", Resumen = "M�s de 50 oficiales completaron el curso avanzado sobre nuevas modalidades de estafas virtuales.", Contenido = "...", ImagenUrl = "https://via.placeholder.com/400x250/0d6efd/FFFFFF?text=Ciberseguridad", FechaPublicacion = DateTime.Now.AddDays(-1) },
                new Noticia { Id = 2, Titulo = "Alerta por Estafas Telef�nicas", Resumen = "La Divisi�n Delitos Econ�micos reitera la importancia de no compartir claves bancarias.", Contenido = "...", ImagenUrl = "https://via.placeholder.com/400x250/dc3545/FFFFFF?text=Alerta", FechaPublicacion = DateTime.Now.AddDays(-3) },
                new Noticia { Id = 3, Titulo = "Nuevos M�viles para Zonas Rurales", Resumen = "Se incorporaron 10 camionetas 4x4 equipadas para mejorar el patrullaje.", Contenido = "...", ImagenUrl = "https://via.placeholder.com/400x250/198754/FFFFFF?text=Nuevos+M�viles", FechaPublicacion = DateTime.Now.AddDays(-5) },
                new Noticia { Id = 4, Titulo = "Reuni�n Clave con Productores Ganaderos", Resumen = "Se acordaron nuevas estrategias de patrullaje conjunto para combatir el abigeato.", Contenido = "...", ImagenUrl = "https://via.placeholder.com/400x250/ffc107/000000?text=Ganader�a", FechaPublicacion = DateTime.Now.AddDays(-7) }
            };
        }
    }
}