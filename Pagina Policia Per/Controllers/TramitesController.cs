using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pagina_Policia_Per.Controllers
{
    public class TramitesController : Controller
    {
        // Usamos una lista estática para simular la base de datos, tal como en las otras secciones.
        private static readonly List<Tramite> _tramites = new List<Tramite>
        {
            new Tramite { Id = 1, Titulo = "Certificado de Reincidencia", ImagenUrl = "/img/tramites/reincidencia.jpg", UrlDestino = "#" },
            new Tramite { Id = 2, Titulo = "Certificado de Buena Conducta", ImagenUrl = "/img/tramites/buena-conducta.jpg", UrlDestino = "#" },
            new Tramite { Id = 3, Titulo = "Armas", ImagenUrl = "/img/tramites/armas.jpg", UrlDestino = "#" },
            new Tramite { Id = 4, Titulo = "Verificación Vehicular", ImagenUrl = "/img/tramites/verificacion.jpg", UrlDestino = "#" },
            new Tramite { Id = 5, Titulo = "Video Vigilancia Urbana", ImagenUrl = "/img/tramites/video-vigilancia.jpg", UrlDestino = "#" },
            new Tramite { Id = 6, Titulo = "Agencias y Vigilancia", ImagenUrl = "/img/tramites/agencias.jpg", UrlDestino = "#" },
            new Tramite { Id = 7, Titulo = "Transacciones Ganaderas", ImagenUrl = "/img/tramites/ganaderas.jpg", UrlDestino = "#" },
            new Tramite { Id = 8, Titulo = "Certificado de Vecindad y Supervivencia", ImagenUrl = "/img/tramites/vecindad.jpg", UrlDestino = "#" },
            new Tramite { Id = 9, Titulo = "Revenido Químico", ImagenUrl = "/img/tramites/revenido.jpg", UrlDestino = "#" },
            new Tramite { Id = 10, Titulo = "Rifas y Bonos", ImagenUrl = "/img/tramites/rifas.jpg", UrlDestino = "#" },
            new Tramite { Id = 11, Titulo = "División Policía Adicional", ImagenUrl = "/img/tramites/adicional.jpg", UrlDestino = "#" }
        };

        public IActionResult Index()
        {
            return View(_tramites);
        }
    }
}