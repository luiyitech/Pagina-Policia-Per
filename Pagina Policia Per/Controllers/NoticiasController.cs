
using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models; // Asegúrate de que este 'using' esté presente
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pagina_Policia_Per.Controllers // Este es tu namespace correcto
{
    public class NoticiasController : Controller
    {
        public IActionResult Index()
        {
            var listaDeNoticias = new List<Noticia>
            {
                new Noticia { Id = 1, Titulo = "Importante Operativo de Seguridad Vial", Resumen = "Controles exhaustivos durante el fin de semana largo para garantizar la seguridad de los viajeros.", ImagenUrl = "https://via.placeholder.com/400x250/0d6efd/FFFFFF?text=Operativo", FechaPublicacion = DateTime.Now.AddDays(-1) },
                new Noticia { Id = 2, Titulo = "Nuevos Cadetes se Suman a la Fuerza", Resumen = "La escuela de oficiales da la bienvenida a la nueva promoción que comenzará su formación este mes.", ImagenUrl = "https://via.placeholder.com/400x250/198754/FFFFFF?text=Cadetes", FechaPublicacion = DateTime.Now.AddDays(-2) },
                new Noticia { Id = 3, Titulo = "Recomendaciones para Prevenir Estafas", Resumen = "La división de delitos económicos emite un comunicado con consejos para proteger a la ciudadanía.", ImagenUrl = "https://via.placeholder.com/400x250/dc3545/FFFFFF?text=Prevención", FechaPublicacion = DateTime.Now.AddDays(-3) },
                new Noticia { Id = 4, Titulo = "Jornada de Prevención en Escuelas", Resumen = "La división de relaciones con la comunidad brindó charlas sobre seguridad digital a estudiantes.", ImagenUrl = "https://via.placeholder.com/400x250/6f42c1/FFFFFF?text=Comunidad", FechaPublicacion = DateTime.Now.AddDays(-4) },
                new Noticia { Id = 5, Titulo = "Incorporación de Nuevas Tecnologías", Resumen = "Se presentaron los nuevos drones que se suman a la vigilancia aérea en zonas rurales.", ImagenUrl = "https://via.placeholder.com/400x250/ffc107/FFFFFF?text=Tecnología", FechaPublicacion = DateTime.Now.AddDays(-5) }
            };

            return View(listaDeNoticias);
        }
    }
}