// Archivo: Controllers/NoticiasController.cs

using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pagina_Policia_Per.Controllers
{
    public class NoticiasController : Controller
    {
        // Acción para mostrar la lista completa de noticias
        public IActionResult Index()
        {
            var listaDeNoticias = _GetNoticias(); // Obtenemos la lista desde nuestro método privado
            return View(listaDeNoticias.OrderByDescending(n => n.FechaPublicacion).ToList());
        }

        // --- ¡NUEVA ACCIÓN PARA MOSTRAR EL DETALLE DE UNA NOTICIA! ---
        // El parámetro 'int id' recibirá el número desde la URL (ej: /Noticias/Detalle/3)
        public IActionResult Detalle(int id)
        {
            var todasLasNoticias = _GetNoticias();

            // Buscamos en la lista la única noticia que coincida con el ID recibido
            var noticia = todasLasNoticias.FirstOrDefault(n => n.Id == id);

            // Si no se encuentra ninguna noticia con ese ID, devolvemos un error 404 (Not Found)
            if (noticia == null)
            {
                return NotFound();
            }

            // Si la encontramos, la pasamos a una nueva vista llamada "Detalle.cshtml"
            return View(noticia);
        }


        // --- Método privado para simular la obtención de datos ---
        private List<Noticia> _GetNoticias()
        {
            return new List<Noticia>
            {
                new Noticia
                {
                    Id = 1,
                    Titulo = "Exitosa Capacitación en Ciberdelitos para Personal de Investigaciones",
                    Resumen = "Más de 50 oficiales completaron el curso avanzado sobre nuevas modalidades de estafas virtuales.", 
                    // ¡AÑADIMOS EL CONTENIDO COMPLETO!
                    Contenido = "Durante tres jornadas intensivas, personal de la División Investigaciones recibió formación de vanguardia en la lucha contra el ciberdelito. Los temas incluyeron phishing, ransomware y técnicas de ingeniería social. El curso fue dictado por expertos en seguridad informática y culminó con ejercicios prácticos de análisis forense digital, fortaleciendo las capacidades de nuestra fuerza para enfrentar los desafíos delictivos del siglo XXI.",
                    ImagenUrl = "https://via.placeholder.com/800x400/0d6efd/FFFFFF?text=Ciberseguridad",
                    FechaPublicacion = DateTime.Now.AddDays(-1)
                },
                new Noticia
                {
                    Id = 2,
                    Titulo = "Nuevos Móviles Refuerzan la Prevención en Zonas Rurales",
                    Resumen = "Se incorporaron 10 camionetas 4x4 equipadas para mejorar el patrullaje.",
                    Contenido = "En un acto presidido por el Jefe de Policía, se hizo entrega oficial de diez nuevas unidades móviles destinadas a las patrullas rurales de la provincia. Estos vehículos, de doble tracción y equipados con comunicación satelital, permitirán un acceso más rápido y seguro a zonas remotas, mejorando significativamente los tiempos de respuesta ante emergencias y reforzando la prevención del abigeato.",
                    ImagenUrl = "https://via.placeholder.com/800x400/198754/FFFFFF?text=Nuevos+Móviles",
                    FechaPublicacion = DateTime.Now.AddDays(-5)
                },
                // ... (y así para el resto de las noticias)
            };
        }
    }
}