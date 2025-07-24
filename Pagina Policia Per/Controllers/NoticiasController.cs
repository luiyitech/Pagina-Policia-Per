// Archivo: Controllers/NoticiasController.cs

using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Services; // Usamos nuestro nuevo servicio

namespace Pagina_Policia_Per.Controllers
{
    [Route("noticias")]
    public class NoticiasController : Controller
    {
        private readonly NoticiaService _noticiaService;

        public NoticiasController(NoticiaService noticiaService)
        {
            _noticiaService = noticiaService;
        }

        [Route("")]
        public IActionResult Index()
        {
            var listaDeNoticias = _noticiaService.GetAllNoticias();
            return View(listaDeNoticias.OrderByDescending(n => n.FechaPublicacion).ToList());
        }

        [Route("{slug}")]
        public IActionResult Detalle(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return BadRequest();

            var noticia = _noticiaService.GetAllNoticias().FirstOrDefault(n => n.Slug == slug);

            if (noticia == null) return NotFound();

            return View(noticia);
        }
    }
}