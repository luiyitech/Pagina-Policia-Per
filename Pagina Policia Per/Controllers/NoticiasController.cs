using Microsoft.AspNetCore.Hosting; // ¡NUEVO! Necesario para IWebHostEnvironment
using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using Pagina_Policia_Per.Services;
using System.IO; // ¡NUEVO! Necesario para trabajar con rutas de archivos
using System.Linq;
using System.Threading.Tasks; // ¡NUEVO! Necesario para los métodos asíncronos

namespace Pagina_Policia_Per.Controllers
{
    [Route("noticias")]
    public class NoticiasController : Controller
    {
        private readonly NoticiaService _noticiaService;
        private readonly IWebHostEnvironment _webHostEnvironment; // ¡NUEVO! Para acceder a wwwroot

        // Modificamos el constructor para recibir también IWebHostEnvironment
        public NoticiasController(NoticiaService noticiaService, IWebHostEnvironment webHostEnvironment)
        {
            _noticiaService = noticiaService;
            _webHostEnvironment = webHostEnvironment; // ¡NUEVO!
        }

        // --- ACCIONES Index() y Detalle() SE MANTIENEN IGUAL ---
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

        // --- ACCIÓN Crear() [GET] SE MANTIENE IGUAL ---
        [Route("crear")]
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }


        // --- ¡ACCIÓN Crear() [POST] COMPLETAMENTE ACTUALIZADA! ---
        [Route("crear")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(NoticiaCreateModel modelo)
        {
            if (ModelState.IsValid)
            {
                string nombreArchivoUnico = null;

                // 1. Verificamos si el usuario subió un archivo
                if (modelo.ImagenPrincipal != null)
                {
                    // 2. Obtenemos la ruta física a la carpeta wwwroot/img/noticias
                    string carpetaUploads = Path.Combine(_webHostEnvironment.WebRootPath, "img", "noticias");

                    // 3. Creamos un nombre de archivo único para evitar que se sobreescriban
                    nombreArchivoUnico = Guid.NewGuid().ToString() + "_" + modelo.ImagenPrincipal.FileName;
                    string rutaArchivo = Path.Combine(carpetaUploads, nombreArchivoUnico);

                    // 4. Guardamos el archivo físicamente en el servidor
                    //    Usamos 'using' para asegurarnos de que el flujo de archivo se cierre correctamente
                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await modelo.ImagenPrincipal.CopyToAsync(fileStream);
                    }
                }

                // 5. Creamos el nuevo objeto Noticia, usando la ruta a la imagen que acabamos de guardar
                var nuevaNoticia = new Noticia
                {
                    Id = _noticiaService.GetAllNoticias().Count + 1,
                    Titulo = modelo.Titulo,
                    Resumen = modelo.Resumen,
                    Contenido = modelo.Contenido,
                    ImagenUrl = "/img/noticias/" + nombreArchivoUnico, // ¡Usamos la nueva ruta!
                    FechaPublicacion = DateTime.Now
                };

                // En un proyecto real, aquí llamaríamos a un método para guardar la 'nuevaNoticia' en la base de datos
                // _noticiaService.AddNoticia(nuevaNoticia);

                TempData["SuccessMessage"] = $"¡La noticia '{nuevaNoticia.Titulo}' se ha creado exitosamente!";
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, volvemos a mostrar el formulario
            return View(modelo);
        }
    }
}