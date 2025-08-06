using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System.Collections.Generic;

namespace Pagina_Policia_Per.Controllers
{
    public class TramitesController : Controller
    {
        private static readonly List<Tramite> _tramites = new()
        {
            new() { Id = 1, Titulo = "Certificado de Reincidencia", ImagenUrl = "/img/tramites/reincidencia.jpg", UrlDestino = "/Tramites/Reincidencia" },
            new() { Id = 2, Titulo = "Certificado de Buena Conducta", ImagenUrl = "/img/tramites/buena-conducta.jpg", UrlDestino = "/Tramites/BuenaConducta" },
            
            // CAMBIO AQUÍ: La UrlDestino ahora apunta a nuestra nueva página de armas.
            new() { Id = 3, Titulo = "Armas", ImagenUrl = "/img/tramites/armas.jpg", UrlDestino = "/Tramites/Armas" },

            new() { Id = 4, Titulo = "Verificación Vehicular", ImagenUrl = "/img/tramites/verificacion.jpg", UrlDestino = "#" },
            new() { Id = 5, Titulo = "Video Vigilancia Urbana", ImagenUrl = "/img/tramites/video-vigilancia.jpg", UrlDestino = "#" },
            new() { Id = 6, Titulo = "Agencias y Vigilancia", ImagenUrl = "/img/tramites/agencias.jpg", UrlDestino = "#" },
            new() { Id = 7, Titulo = "Transacciones Ganaderas", ImagenUrl = "/img/tramites/ganaderas.jpg", UrlDestino = "#" },
            new() { Id = 8, Titulo = "Certificado de Vecindad y Supervivencia", ImagenUrl = "/img/tramites/vecindad.jpg", UrlDestino = "#" },
            new() { Id = 9, Titulo = "Revenido Químico", ImagenUrl = "/img/tramites/revenido.jpg", UrlDestino = "#" },
            new() { Id = 10, Titulo = "Rifas y Bonos", ImagenUrl = "/img/tramites/rifas.jpg", UrlDestino = "#" },
            new() { Id = 11, Titulo = "División Policía Adicional", ImagenUrl = "/img/tramites/adicional.jpg", UrlDestino = "#" }
        };

        public IActionResult Index()
        {
            return View(_tramites);
        }

        public IActionResult Reincidencia()
        {
            return View();
        }

        public IActionResult BuenaConducta()
        {
            return View();
        }

        // ACCIÓN NUEVA AÑADIDA:
        // Responde a la URL /Tramites/Armas y muestra la nueva vista.
        public IActionResult Armas()
        {
            return View();
        }
    }
}