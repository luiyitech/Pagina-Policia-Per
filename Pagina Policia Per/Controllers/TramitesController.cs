using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System.Collections.Generic;

namespace Pagina_Policia_Per.Controllers
{
    public class TramitesController : Controller
    {
        // Esta es la sintaxis simplificada que recomienda Visual Studio
        private static readonly List<Tramite> _tramites = new()
        {
            // CAMBIO AQUÍ: La UrlDestino ahora apunta a nuestra nueva página de detalle.
            new() { Id = 1, Titulo = "Certificado de Reincidencia", ImagenUrl = "/img/tramites/reincidencia.jpg", UrlDestino = "/Tramites/Reincidencia" },
            
            // El resto de los trámites permanecen igual por ahora.
            new() { Id = 2, Titulo = "Certificado de Buena Conducta", ImagenUrl = "/img/tramites/buena-conducta.jpg", UrlDestino = "#" },
            new() { Id = 3, Titulo = "Armas", ImagenUrl = "/img/tramites/armas.jpg", UrlDestino = "#" },
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

        // ACCIÓN NUEVA AÑADIDA:
        // Esta acción es la que se ejecuta cuando el usuario va a /Tramites/Reincidencia
        // y devuelve la vista Reincidencia.cshtml.
        public IActionResult Reincidencia()
        {
            return View();
        }
    }
}