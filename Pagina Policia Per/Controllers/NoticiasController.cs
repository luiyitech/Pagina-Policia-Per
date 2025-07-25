using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models; // ¡Importante! Añadimos el 'using' para el nuevo ViewModel
using Pagina_Policia_Per.Services;
using System.Linq;

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

        // --- ACCIÓN PARA MOSTRAR LA LISTA DE NOTICIAS ---
        // URL: /noticias
        [Route("")]
        public IActionResult Index()
        {
            var listaDeNoticias = _noticiaService.GetAllNoticias();
            return View(listaDeNoticias.OrderByDescending(n => n.FechaPublicacion).ToList());
        }

        // --- ACCIÓN PARA MOSTRAR EL DETALLE DE UNA NOTICIA ---
        // URL: /noticias/mi-slug-de-noticia
        [Route("{slug}")]
        public IActionResult Detalle(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return BadRequest();

            var noticia = _noticiaService.GetAllNoticias().FirstOrDefault(n => n.Slug == slug);

            if (noticia == null) return NotFound();

            return View(noticia);
        }

        // --- ACCIÓN [GET] PARA MOSTRAR EL FORMULARIO DE CREACIÓN ---
        // URL: /noticias/crear
        [Route("crear")]
        [HttpGet] // Este método solo responde a peticiones GET (cuando se visita la URL)
        public IActionResult Crear()
        {
            // Simplemente mostramos la vista con el formulario en blanco.
            return View();
        }

        // --- ACCIÓN [POST] PARA PROCESAR LOS DATOS DEL FORMULARIO ---
        // URL: /noticias/crear (cuando el formulario se envía)
        [Route("crear")]
        [HttpPost] // Este método solo responde a peticiones POST
        [ValidateAntiForgeryToken] // Medida de seguridad importante contra ataques CSRF
        public IActionResult Crear(NoticiaCreateModel modelo)
        {
            // 1. Verificamos si el modelo que llegó es válido (cumple las reglas [Required], etc.)
            if (ModelState.IsValid)
            {
                // --- AQUÍ IRÍA LA LÓGICA PARA GUARDAR EN LA BASE DE DATOS ---
                // Por ahora, solo simulamos la creación.

                // NOTA: Para que la nueva noticia realmente aparezca en la lista,
                // necesitaríamos modificar el NoticiaService para que pueda añadir
                // elementos a su lista interna. Por ahora, este paso es conceptual.

                // Creamos un nuevo objeto Noticia con los datos del formulario.
                var nuevaNoticia = new Noticia
                {
                    Id = _noticiaService.GetAllNoticias().Count + 1, // Asignamos un nuevo ID (temporal)
                    Titulo = modelo.Titulo,
                    Resumen = modelo.Resumen,
                    Contenido = modelo.Contenido,
                    ImagenUrl = modelo.ImagenUrl,
                    FechaPublicacion = DateTime.Now
                    // El Slug se generaría al guardarlo en el servicio/base de datos.
                };

                // _noticiaService.AddNoticia(nuevaNoticia); // <-- Esto lo implementaríamos en el futuro

                // 2. Si todo sale bien, redirigimos al usuario a la página de la lista de noticias.
                //    Usamos TempData para enviar un mensaje de éxito a la siguiente página.
                TempData["SuccessMessage"] = $"¡La noticia '{nuevaNoticia.Titulo}' se ha creado exitosamente!";
                return RedirectToAction("Index");
            }

            // 3. Si el modelo no es válido (ej. falta el título), volvemos a mostrar el formulario.
            //    ASP.NET se encargará de mostrar los mensajes de error junto a cada campo.
            return View(modelo);
        }
    }
}