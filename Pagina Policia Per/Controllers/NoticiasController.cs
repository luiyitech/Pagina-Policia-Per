using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Pagina_Policia_Per.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Text;

namespace Pagina_Policia_Per.Controllers
{
    public class NoticiasController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NoticiasController> _logger;

        public NoticiasController(IWebHostEnvironment hostEnvironment, ApplicationDbContext context, ILogger<NoticiasController> logger = null)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _logger = logger;
        }

        private static string GenerateSlug(string title, int maxLength = 80)
        {
            if (string.IsNullOrEmpty(title)) return "sin-titulo";
            var normalizedString = title.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            string sanitizedTitle = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
            sanitizedTitle = Regex.Replace(sanitizedTitle, @"[^a-z0-9\s-]", "");
            sanitizedTitle = Regex.Replace(sanitizedTitle, @"\s+", " ").Trim();
            sanitizedTitle = sanitizedTitle.Replace(" ", "-");
            sanitizedTitle = Regex.Replace(sanitizedTitle, @"-+", "-");
            if (sanitizedTitle.Length > maxLength)
            {
                sanitizedTitle = sanitizedTitle.Substring(0, maxLength);
                if (sanitizedTitle.LastIndexOf('-') > 0)
                {
                    sanitizedTitle = sanitizedTitle.Substring(0, sanitizedTitle.LastIndexOf('-'));
                }
            }
            return sanitizedTitle;
        }

        public async Task<IActionResult> Index()
        {
            var primerasNoticias = await _context.Noticia.OrderByDescending(n => n.FechaPublicacion).Take(3).ToListAsync();
            return View(primerasNoticias);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            var todasLasNoticias = await _context.Noticia.OrderByDescending(n => n.FechaPublicacion).ToListAsync();
            return View(todasLasNoticias);
        }

        [HttpGet]
        public async Task<IActionResult> GetNoticias(int page = 1, int pageSize = 3)
        {
            var noticias = await _context.Noticia.OrderByDescending(n => n.FechaPublicacion).Skip((page - 1) * pageSize).Take(pageSize).Select(n => new { n.Titulo, n.Resumen, n.ImagenUrl, Fecha = n.FechaPublicacion.ToString("dd 'de' MMMM, yyyy"), Url = $"/Noticias/Detalle/{n.Slug}" }).ToListAsync();
            return new JsonResult(noticias);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Crear()
        {
            return View(new NoticiaCreateModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(NoticiaCreateModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.ImagenPrincipal != null && model.ImagenPrincipal.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img/noticias");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImagenPrincipal.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    try
                    {
                        Directory.CreateDirectory(uploadsFolder);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImagenPrincipal.CopyToAsync(fileStream);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"Error al subir el archivo principal: {filePath}");
                        ModelState.AddModelError("ImagenPrincipal", "Error al subir el archivo de imagen.");
                        return View(model);
                    }
                }
                var nuevaNoticia = new Noticia { Titulo = model.Titulo, Resumen = model.Resumen, Contenido = model.Contenido, ImagenUrl = uniqueFileName != null ? $"/img/noticias/{uniqueFileName}" : "/img/noticias/default.jpg", FechaPublicacion = DateTime.Now, Slug = GenerateSlug(model.Titulo) };
                try
                {
                    _context.Add(nuevaNoticia);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "¡La noticia se ha creado correctamente!";
                    return RedirectToAction(nameof(Admin));
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error al guardar la nueva noticia en la base de datos.");
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar la noticia.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("Noticias/Detalle/{slug}")]
        public async Task<IActionResult> Detalle(string slug)
        {
            var noticia = await _context.Noticia.FirstOrDefaultAsync(n => n.Slug == slug);
            if (noticia == null) return NotFound();
            return View(noticia);
        }

        [HttpGet]
        [Route("Noticias/Editar/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();
            var noticia = await _context.Noticia.FirstOrDefaultAsync(m => m.Id == id);
            if (noticia == null) return NotFound();
            return View(noticia);
        }

        [HttpPost]
        [Route("Noticias/Editar/{id:int}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Noticia noticia, bool eliminarImagen, IFormFile NuevaImagenPrincipal)
        {
            if (id != noticia.Id) return NotFound();

            var noticiaToUpdate = await _context.Noticia.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (noticiaToUpdate == null) return NotFound();

            if (noticiaToUpdate.Titulo != noticia.Titulo)
            {
                noticia.Slug = GenerateSlug(noticia.Titulo);
            }
            else
            {
                noticia.Slug = noticiaToUpdate.Slug;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string oldImageUrl = noticiaToUpdate.ImagenUrl;
                    noticiaToUpdate = noticia;

                    if (NuevaImagenPrincipal != null && NuevaImagenPrincipal.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(oldImageUrl) && !oldImageUrl.EndsWith("default.jpg"))
                        {
                            string oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, oldImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                try { System.IO.File.Delete(oldImagePath); } catch (Exception ex) { _logger?.LogError(ex, $"Error al eliminar imagen antigua: {oldImagePath}"); }
                            }
                        }
                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img/noticias");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(NuevaImagenPrincipal.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        Directory.CreateDirectory(uploadsFolder);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await NuevaImagenPrincipal.CopyToAsync(fileStream);
                        }
                        noticiaToUpdate.ImagenUrl = $"/img/noticias/{uniqueFileName}";
                    }
                    else if (eliminarImagen)
                    {
                        if (!string.IsNullOrEmpty(oldImageUrl) && !oldImageUrl.EndsWith("default.jpg"))
                        {
                            string oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, oldImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                try { System.IO.File.Delete(oldImagePath); } catch (Exception ex) { _logger?.LogError(ex, $"Error al eliminar imagen antigua: {oldImagePath}"); }
                            }
                        }
                        noticiaToUpdate.ImagenUrl = null;
                    }
                    else
                    {
                        noticiaToUpdate.ImagenUrl = oldImageUrl;
                    }

                    _context.Update(noticiaToUpdate);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "¡La noticia se ha actualizado correctamente!";
                    return RedirectToAction(nameof(Admin));
                }
                catch (DbUpdateConcurrencyException) { if (!NoticiaExists(noticia.Id)) { return NotFound(); } else { throw; } }
            }
            return View(noticia);
        }

        // ===================================================================
        //           NUEVAS ACCIONES PARA "ELIMINAR NOTICIA"
        // ===================================================================
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var noticia = await _context.Noticia.FirstOrDefaultAsync(m => m.Id == id);
            if (noticia == null)
            {
                return NotFound();
            }
            return View(noticia);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var noticia = await _context.Noticia.FindAsync(id);
            if (noticia == null)
            {
                return RedirectToAction(nameof(Admin));
            }
            if (!string.IsNullOrEmpty(noticia.ImagenUrl) && !noticia.ImagenUrl.EndsWith("default.jpg"))
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, noticia.ImagenUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    try { System.IO.File.Delete(imagePath); }
                    catch (Exception ex) { _logger?.LogError(ex, $"Error al eliminar archivo de imagen: {imagePath}"); }
                }
            }
            _context.Noticia.Remove(noticia);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "¡La noticia se ha eliminado correctamente!";
            return RedirectToAction(nameof(Admin));
        }
        // ===================================================================

        private bool NoticiaExists(int id)
        {
            return _context.Noticia.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("Noticias/UploadImage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload == null || upload.Length == 0) return BadRequest(new { uploaded = 0, error = new { message = "No se ha seleccionado ningún archivo para subir." } });
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img/noticias");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(upload.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }
                return new JsonResult(new { uploaded = 1, fileName = uniqueFileName, url = $"/img/noticias/{uniqueFileName}" });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error al guardar la imagen subida por CKEditor: {filePath}");
                return StatusCode(500, new { uploaded = 0, error = new { message = $"Error al guardar el archivo: {ex.Message}" } });
            }
        }
    }
}