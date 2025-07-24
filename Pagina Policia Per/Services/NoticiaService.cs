// Archivo: Services/NoticiaService.cs

using Pagina_Policia_Per.Models;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Pagina_Policia_Per.Services
{
    public class NoticiaService
    {
        private readonly List<Noticia> _noticias;

        public NoticiaService()
        {
            _noticias = new List<Noticia>
            {
                new Noticia
                {
                    Id = 1,
                    Titulo = "Exitosa Capacitación en Ciberdelitos para Personal de Investigaciones",
                    Resumen = "Más de 50 oficiales completaron el curso avanzado sobre nuevas modalidades de estafas virtuales.",
                    Contenido = "Durante tres jornadas intensivas...",
                    ImagenUrl = "/img/noticias/ciberseguridad.jpg", // <-- CORREGIDO
                    FechaPublicacion = DateTime.Now.AddDays(-1)
                },
                new Noticia
                {
                    Id = 2,
                    Titulo = "Nuevos Móviles Refuerzan la Prevención en Zonas Rurales",
                    Resumen = "Se incorporaron 10 camionetas 4x4 equipadas para mejorar el patrullaje.",
                    Contenido = "En un acto presidido por el Jefe de Policía...",
                    ImagenUrl = "/img/noticias/nuevos-moviles.jpg", // <-- CORREGIDO
                    FechaPublicacion = DateTime.Now.AddDays(-5)
                },
                 new Noticia
                {
                    Id = 3,
                    Titulo = "Alerta por Estafas Telefónicas: No Brinde Datos Personales",
                    Resumen = "La División Delitos Económicos reitera la importancia de no compartir claves bancarias.",
                    Contenido = "Ante el aumento de denuncias por estafas telefónicas...",
                    ImagenUrl = "/img/noticias/alerta-estafas.jpg", // <-- CORREGIDO
                    FechaPublicacion = DateTime.Now.AddDays(-3)
                },
            };

            foreach (var noticia in _noticias)
            {
                noticia.Slug = GenerateSlug(noticia.Titulo);
            }
        }

        public List<Noticia> GetAllNoticias() => _noticias;

        private static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrEmpty(phrase)) return string.Empty;
            string str = phrase.ToLowerInvariant();
            str = string.Concat(str.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = Regex.Replace(str, @"\s", "-");
            str = Regex.Replace(str, @"-+", "-");
            return str;
        }
    }
}