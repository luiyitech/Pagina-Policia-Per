using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pagina_Policia_Per.Controllers
{
    public class NoticiasController : Controller
    {
        public static readonly List<Noticia> _allNoticias = new List<Noticia>
        {
            new Noticia { Id = 1, Titulo = "Nuevos Patrulleros para la Jefatura Departamental", Resumen = "Se incorporaron 10 nuevas unidades móviles para reforzar la seguridad...", Contenido = "...", ImagenUrl = "/img/noticias/nuevos-moviles.jpg", FechaPublicacion = new DateTime(2025, 7, 30), Slug = "nuevos-patrulleros-jefatura" },
            new Noticia { Id = 2, Titulo = "Exitosa Capacitación en Técnicas de Reanimación", Resumen = "Más de 50 oficiales completaron el curso de RCP y primeros auxilios...", Contenido = "...", ImagenUrl = "/img/noticias/rcp.jpg", FechaPublicacion = new DateTime(2025, 7, 28), Slug = "exitosa-capacitacion-rcp" },
            new Noticia { Id = 3, Titulo = "La División Canina Realizó una Demostración en Escuelas", Resumen = "En el marco del programa de acercamiento a la comunidad, la división canes visitó la escuela N°5...", Contenido = "...", ImagenUrl = "/img/noticias/canina.jpg", FechaPublicacion = new DateTime(2025, 7, 25), Slug = "division-canina-demostracion" },
            new Noticia { Id = 4, Titulo = "Incautaron Mercadería de Contrabando en Ruta 14", Resumen = "Personal de la Dirección de Prevención y Seguridad Vial detuvo un camión con electrónicos...", Contenido = "...", ImagenUrl = "/img/noticias/contrabando.webp", FechaPublicacion = new DateTime(2025, 7, 22), Slug = "incautan-mercaderia-contrabando" },
            new Noticia { Id = 5, Titulo = "Reunión Clave con Vecinos del Barrio San Martín", Resumen = "El Jefe Departamental se reunió con la comisión vecinal para coordinar nuevas estrategias...", Contenido = "...", ImagenUrl = "/img/noticias/vecinal.webp", FechaPublicacion = new DateTime(2025, 7, 20), Slug = "reunion-vecinos-san-martin" },
            new Noticia { Id = 6, Titulo = "Abierta la Inscripción para la Escuela de Oficiales", Resumen = "Se encuentra abierto el período de inscripción para el ciclo lectivo 2026...", Contenido = "...", ImagenUrl = "/img/noticias/cadetes.webp", FechaPublicacion = new DateTime(2025, 7, 18), Slug = "inscripcion-escuela-oficiales" }
        };

        public IActionResult Index()
        {
            var noticiasIniciales = new List<Noticia>(); // Creamos una lista vacía
            return View(noticiasIniciales); // Le pasamos la lista vacía a la vista
        }

        [HttpGet]
        public async Task<IActionResult> GetNoticias(int page = 1, int pageSize = 3)
        {
            await Task.Delay(500);

            var noticias = _allNoticias
                .OrderByDescending(n => n.FechaPublicacion) // Ordenamos por fecha
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new { // Devolvemos un objeto anónimo solo con los datos que necesitamos
                    Titulo = n.Titulo,
                    Resumen = n.Resumen,
                    ImagenUrl = n.ImagenUrl,
                    Fecha = n.FechaPublicacion.ToString("dd 'de' MMMM, yyyy"), // Formateamos la fecha
                    Url = $"/Noticias/Detalle/{n.Slug}" // Creamos la URL
                })
                .ToList();

            return new JsonResult(noticias);
        }
    }
}