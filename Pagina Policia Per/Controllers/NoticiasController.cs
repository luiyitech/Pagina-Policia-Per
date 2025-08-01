using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // ¡MUY IMPORTANTE! Necesario para la ruta de archivos.

namespace Pagina_Policia_Per.Controllers
{
    public class NoticiasController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        // Nuestra lista "simulada" de base de datos. La hacemos pública para que el HomeController la pueda usar.
        public static readonly List<Noticia> _allNoticias = new List<Noticia>
        {
            new Noticia { Id = 1, Titulo = "Nuevos Patrulleros para la Jefatura Departamental", Resumen = "Se incorporaron 10 nuevas unidades móviles para reforzar la seguridad...", Contenido = "Contenido completo de la noticia sobre los patrulleros.", ImagenUrl = "/img/noticias/nuevos-moviles.jpg", FechaPublicacion = new DateTime(2025, 7, 30), Slug = "nuevos-patrulleros-jefatura" },
            new Noticia { Id = 2, Titulo = "Exitosa Capacitación en Técnicas de Reanimación", Resumen = "Más de 50 oficiales completaron el curso de RCP y primeros auxilios...", Contenido = "Contenido completo de la noticia sobre la capacitación.", ImagenUrl = "/img/noticias/rcp.jpg", FechaPublicacion = new DateTime(2025, 7, 28), Slug = "exitosa-capacitacion-rcp" },
            new Noticia { Id = 3, Titulo = "La División Canina Realizó una Demostración en Escuelas", Resumen = "En el marco del programa de acercamiento a la comunidad, la división canes visitó la escuela N°5...", Contenido = "Contenido completo de la noticia sobre la división canina.", ImagenUrl = "/img/noticias/canina.jpg", FechaPublicacion = new DateTime(2025, 7, 25), Slug = "division-canina-demostracion" },
            new Noticia { Id = 4, Titulo = "Incautaron Mercadería de Contrabando en Ruta 14", Resumen = "Personal de la Dirección de Prevención y Seguridad Vial detuvo un camión con electrónicos...", Contenido = "...", ImagenUrl = "/img/noticias/contrabando.webp", FechaPublicacion = new DateTime(2025, 7, 22), Slug = "incautan-mercaderia-contrabando" },
            new Noticia { Id = 5, Titulo = "Reunión Clave con Vecinos del Barrio San Martín", Resumen = "El Jefe Departamental se reunió con la comisión vecinal para coordinar nuevas estrategias...", Contenido = "...", ImagenUrl = "/img/noticias/vecinal.webp", FechaPublicacion = new DateTime(2025, 7, 20), Slug = "reunion-vecinos-san-martin" },
            new Noticia { Id = 6, Titulo = "Abierta la Inscripción para la Escuela de Oficiales", Resumen = "Se encuentra abierto el período de inscripción para el ciclo lectivo 2026...", Contenido = "...", ImagenUrl = "/img/noticias/cadetes.webp", FechaPublicacion = new DateTime(2025, 7, 18), Slug = "inscripcion-escuela-oficiales" }
        };

        // Inyectamos IWebHostEnvironment para poder saber dónde está la carpeta wwwroot
        public NoticiasController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        // --- ACCIÓN PARA LA PÁGINA DE LISTADO (SCROLL INFINITO) ---
        public IActionResult Index()
        {
            return View();
        }

        // --- API PARA EL SCROLL INFINITO ---
        [HttpGet]
        public async Task<IActionResult> GetNoticias(int page = 1, int pageSize = 3)
        {
            await Task.Delay(500);
            var noticias = _allNoticias
                .OrderByDescending(n => n.FechaPublicacion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new {
                    Titulo = n.Titulo,
                    Resumen = n.Resumen,
                    ImagenUrl = n.ImagenUrl,
                    Fecha = n.FechaPublicacion.ToString("dd 'de' MMMM, yyyy"),
                    Url = $"/Noticias/Detalle/{n.Slug}"
                }).ToList();
            return new JsonResult(noticias);
        }

        // ===================================================================
        //           MÉTODOS AVANZADOS PARA "CREAR NOTICIA"
        // ===================================================================

        // --- 1. MUESTRA EL FORMULARIO VACÍO ---
        [HttpGet]
        public IActionResult Crear()
        {
            // Devuelve la vista "Crear.cshtml" esperando un modelo NoticiaCreateModel
            return View(new NoticiaCreateModel());
        }

        // --- 2. RECIBE LOS DATOS Y LA IMAGEN DEL FORMULARIO ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(NoticiaCreateModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // --- LÓGICA PARA SUBIR LA IMAGEN ---
                if (model.ImagenPrincipal != null)
                {
                    // 1. Define la carpeta donde se guardarán las imágenes
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img/noticias");
                    // 2. Crea un nombre de archivo único para evitar colisiones
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagenPrincipal.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // 3. Guarda el archivo en el servidor
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImagenPrincipal.CopyToAsync(fileStream);
                    }
                }

                // Creamos la noticia final a partir de los datos del formulario
                var nuevaNoticia = new Noticia
                {
                    Id = _allNoticias.Any() ? _allNoticias.Max(n => n.Id) + 1 : 1,
                    Titulo = model.Titulo,
                    Resumen = model.Resumen,
                    Contenido = model.Contenido,
                    ImagenUrl = uniqueFileName != null ? $"/img/noticias/{uniqueFileName}" : "/img/noticias/default.jpg", // Usa una imagen por defecto si no se sube ninguna
                    FechaPublicacion = DateTime.Now,
                    Slug = (model.Titulo ?? "nueva-noticia").ToLower().Replace(" ", "-").Substring(0, Math.Min((model.Titulo ?? "").Length, 50)) // Slug simple y seguro
                };

                _allNoticias.Insert(0, nuevaNoticia); // La añadimos al principio de la lista para que aparezca primera

                TempData["SuccessMessage"] = "¡La noticia se ha creado correctamente!";
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, vuelve a mostrar el formulario con los errores
            return View(model);
        }
    }
}