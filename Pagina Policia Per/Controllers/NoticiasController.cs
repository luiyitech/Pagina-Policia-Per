using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Pagina_Policia_Per.Data; // <--- Asegúrate de tener esta directiva using para ApplicationDbContext
using Microsoft.EntityFrameworkCore; // <--- Necesario para usar métodos de extensión de EF Core como ToListAsync() o AddAsync()

namespace Pagina_Policia_Per.Controllers
{
    public class NoticiasController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context; // <--- Agregamos una variable para el contexto de base de datos

        // Eliminamos la lista estática _allNoticias

        // Inyectamos IWebHostEnvironment Y ApplicationDbContext en el constructor
        public NoticiasController(IWebHostEnvironment hostEnvironment, ApplicationDbContext context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context; // <--- Asignamos el contexto inyectado
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
            // await Task.Delay(500); // Ya no necesitamos este delay si leemos de una "base de datos"

            // --- Modificado para leer del DbContext ---
            var noticias = await _context.Noticia
                .OrderByDescending(n => n.FechaPublicacion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new {
                    Titulo = n.Titulo,
                    Resumen = n.Resumen,
                    ImagenUrl = n.ImagenUrl,
                    Fecha = n.FechaPublicacion.ToString("dd 'de' MMMM, yyyy"),
                    Url = $"/Noticias/Detalle/{n.Slug}"
                }).ToListAsync(); // <--- Usamos ToListAsync() para operar asíncronamente con EF Core

            return new JsonResult(noticias);
        }

        // ===================================================================
        //           MÉTODOS AVANZADOS PARA "CREAR NOTICIA"
        // ===================================================================

        // --- 1. MUESTRA EL FORMULARIO VACÍO ---
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Crear()
        {
            return View(new NoticiaCreateModel());
        }

        // --- 2. RECIBE LOS DATOS Y LA IMAGEN DEL FORMULARIO ---
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(NoticiaCreateModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // --- LÓGICA PARA SUBIR LA IMAGEN ---
                if (model.ImagenPrincipal != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img/noticias");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagenPrincipal.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImagenPrincipal.CopyToAsync(fileStream);
                    }
                }

                // Creamos la noticia final a partir de los datos del formulario
                var nuevaNoticia = new Noticia
                {
                    // Ya no necesitamos generar manualmente el Id, la base de datos (EF Core) lo hará
                    // Id = _allNoticias.Any() ? _allNoticias.Max(n => n.Id) + 1 : 1,
                    Titulo = model.Titulo,
                    Resumen = model.Resumen,
                    Contenido = model.Contenido,
                    ImagenUrl = uniqueFileName != null ? $"/img/noticias/{uniqueFileName}" : "/img/noticias/default.jpg",
                    FechaPublicacion = DateTime.Now,
                    Slug = (string.IsNullOrEmpty(model.Titulo) ? "nueva-noticia" : model.Titulo.ToLower().Replace(" ", "-")).Substring(0, Math.Min((model.Titulo ?? "").Length, 50))
                };

                // --- Modificado para agregar al DbContext ---
                _context.Add(nuevaNoticia); // Prepara el objeto para ser guardado
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos (en memoria por ahora)
                // --- Fin de la modificación ---

                TempData["SuccessMessage"] = "¡La noticia se ha creado correctamente!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // --- ACCIÓN PARA MOSTRAR EL DETALLE DE UNA NOTICIA ---
        // Modificado para leer del DbContext
        public async Task<IActionResult> Detalle(string slug)
        {
            // --- Modificado para leer del DbContext ---
            var noticia = await _context.Noticia.FirstOrDefaultAsync(n => n.Slug == slug);
            // --- Fin de la modificación ---

            if (noticia == null)
            {
                return NotFound();
            }
            return View(noticia);
        }
    }
}
