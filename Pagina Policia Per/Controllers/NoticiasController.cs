using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization; // Necesario para la función de Slug
using System.Text;          // Necesario para la función de Slug
using System.Text.RegularExpressions; // Necesario para la función de Slug

namespace Pagina_Policia_Per.Controllers
{
    // Este atributo define la ruta base para todas las acciones en este controlador.
    // Ahora, todas las URLs comenzarán con "/noticias".
    [Route("noticias")]
    public class NoticiasController : Controller
    {
        // Este atributo define la ruta para la acción Index.
        // Al estar vacío (""), significa que la URL "/noticias" ejecutará este método.
        [Route("")]
        public IActionResult Index()
        {
            var listaDeNoticias = _GetNoticias(); // Obtenemos la lista desde nuestro método privado
            return View(listaDeNoticias.OrderByDescending(n => n.FechaPublicacion).ToList());
        }

        // Este atributo define la ruta para la acción Detalle.
        // "{slug}" es un parámetro. La URL "/noticias/mi-slug-de-noticia" ejecutará este método,
        // y "mi-slug-de-noticia" se pasará como el valor del parámetro 'slug'.
        [Route("{slug}")]
        public IActionResult Detalle(string slug)
        {
            // Verificamos que el slug no sea nulo o vacío
            if (string.IsNullOrEmpty(slug))
            {
                return BadRequest(); // Devuelve un error 400 (Petición incorrecta)
            }

            var todasLasNoticias = _GetNoticias();

            // Buscamos en la lista la única noticia que coincida con el Slug recibido
            var noticia = todasLasNoticias.FirstOrDefault(n => n.Slug == slug);

            // Si no se encuentra ninguna noticia con ese Slug, devolvemos un error 404 (No Encontrado)
            if (noticia == null)
            {
                return NotFound();
            }

            // Si la encontramos, la pasamos a la vista "Detalle.cshtml"
            return View(noticia);
        }

        // --- Método privado para simular la obtención de datos ---
        // Este método ahora también se encarga de generar el Slug para cada noticia.
        private List<Noticia> _GetNoticias()
        {
            var noticias = new List<Noticia>
            {
                new Noticia
                {
                    Id = 1,
                    Titulo = "Exitosa Capacitación en Ciberdelitos para Personal de Investigaciones",
                    Resumen = "Más de 50 oficiales completaron el curso avanzado sobre nuevas modalidades de estafas virtuales.",
                    Contenido = "Durante tres jornadas intensivas, personal de la División Investigaciones recibió formación de vanguardia en la lucha contra el ciberdelito. Los temas incluyeron phishing, ransomware y técnicas de ingeniería social. El curso fue dictado por expertos en seguridad informática y culminó con ejercicios prácticos de análisis forense digital, fortaleciendo las capacidades de nuestra fuerza para enfrentar los desafíos delictivos del siglo XXI.",
                    ImagenUrl = "~/img/noticias/ciberseguridad.jpg",
                    FechaPublicacion = DateTime.Now.AddDays(-1)
                },
                new Noticia
                {
                    Id = 2,
                    Titulo = "Nuevos Móviles Refuerzan la Prevención en Zonas Rurales",
                    Resumen = "Se incorporaron 10 camionetas 4x4 equipadas para mejorar el patrullaje.",
                    Contenido = "En un acto presidido por el Jefe de Policía, se hizo entrega oficial de diez nuevas unidades móviles destinadas a las patrullas rurales de la provincia. Estos vehículos, de doble tracción y equipados con comunicación satelital, permitirán un acceso más rápido y seguro a zonas remotas, mejorando significativamente los tiempos de respuesta ante emergencias y reforzando la prevención del abigeato.",
                    ImagenUrl = "~/img/noticias/Moviles-rurales.jpg",
                    FechaPublicacion = DateTime.Now.AddDays(-5)
                },
                 new Noticia
                {
                    Id = 3,
                    Titulo = "Alerta por Estafas Telefónicas: No Brinde Datos Personales",
                    Resumen = "La División Delitos Económicos reitera la importancia de no compartir claves bancarias o códigos de verificación por teléfono.",
                    Contenido = "Ante el aumento de denuncias por estafas telefónicas, la Policía de Entre Ríos recuerda a la comunidad que ninguna entidad bancaria o gubernamental solicitará claves, contraseñas o códigos de seguridad por teléfono o WhatsApp. Desconfíe de premios inesperados y no instale aplicaciones a pedido de desconocidos. Ante la duda, corte la comunicación y contacte a la entidad por sus canales oficiales.",
                    ImagenUrl = "~/img/noticias/estafas.jpg",
                    FechaPublicacion = DateTime.Now.AddDays(-3)
                },
            };

            // Generamos el Slug para cada noticia en la lista antes de devolverla.
            foreach (var noticia in noticias)
            {
                noticia.Slug = GenerateSlug(noticia.Titulo);
            }

            return noticias;
        }

        // --- FUNCIÓN PRIVADA PARA GENERAR SLUGS ---
        // Transforma un título como "¡Hola Mundo!" en un slug como "hola-mundo"
        private static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return string.Empty;

            string str = phrase.ToLowerInvariant();

            // 1. Normaliza para quitar acentos y diacríticos
            str = string.Concat(str.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));

            // 2. Quita caracteres inválidos
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // 3. Convierte múltiples espacios en un solo guión
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // 4. Reemplaza espacios con guiones
            str = Regex.Replace(str, @"\s", "-");

            // 5. Opcional: Asegura que no haya guiones dobles
            str = Regex.Replace(str, @"-+", "-");

            return str;
        }
    }
}