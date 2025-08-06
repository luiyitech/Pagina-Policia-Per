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
            new() { Id = 3, Titulo = "Armas", ImagenUrl = "/img/tramites/armas.jpg", UrlDestino = "/Tramites/Armas" },
            new() { Id = 4, Titulo = "Verificación Vehicular", ImagenUrl = "/img/tramites/verificacion.jpg", UrlDestino = "/Tramites/VerificacionVehicular" },
            new() { Id = 5, Titulo = "Agencias y Vigilancia", ImagenUrl = "/img/tramites/agencias.jpg", UrlDestino = "/Tramites/AgenciasVigilancia" },
            new() { Id = 6, Titulo = "Transacciones Ganaderas", ImagenUrl = "/img/tramites/ganaderas.jpg", UrlDestino = "/Tramites/TransaccionesGanaderas" },
            new() { Id = 7, Titulo = "Certificado de Vecindad y Supervivencia", ImagenUrl = "/img/tramites/vecindad.jpg", UrlDestino = "/Tramites/VecindadSupervivencia" },
            new() { Id = 8, Titulo = "Revenido Químico", ImagenUrl = "/img/tramites/revenido.jpg", UrlDestino = "/Tramites/RevenidoQuimico" },
            new() { Id = 9, Titulo = "Rifas y Bonos", ImagenUrl = "/img/tramites/rifas.jpg", UrlDestino = "/Tramites/RifasBonos" },
            
            // CAMBIO AQUÍ: La UrlDestino ahora apunta a nuestra nueva página.
            new() { Id = 10, Titulo = "División Policía Adicional", ImagenUrl = "/img/tramites/adicional.jpg", UrlDestino = "/Tramites/PoliciaAdicional" }
        };

        public IActionResult Index() { return View(_tramites); }
        public IActionResult Reincidencia() { return View(); }
        public IActionResult BuenaConducta() { return View(); }
        public IActionResult Armas() { return View(); }
        public IActionResult VerificacionVehicular() { return View(); }
        public IActionResult AgenciasVigilancia() { return View(); }
        public IActionResult TransaccionesGanaderas() { return View(); }
        public IActionResult VecindadSupervivencia() { return View(); }
        public IActionResult RevenidoQuimico() { return View(); }
        public IActionResult RifasBonos() { return View(); }

        // ACCIÓN NUEVA AÑADIDA:
        public IActionResult PoliciaAdicional()
        {
            return View();
        }
    }
}