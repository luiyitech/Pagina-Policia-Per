using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models; // ¡Importante añadir esta línea para usar el modelo que creamos!
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pagina_Policia_Per.Controllers
{
    public class InstitucionalController : Controller
    {
        // Esta lista simula una base de datos. Aquí es donde configuras TODAS tus imágenes.
        // ¡RECUERDA REEMPLAZAR ESTO CON TUS IMÁGENES Y DESCRIPCIONES REALES!
        private static readonly List<GaleriaImage> _allImages = new List<GaleriaImage>
        {
            // Reemplaza los nombres de archivo (foto1.jpg, etc.) y las descripciones.
            // Asegúrate que los archivos existan en /wwwroot/img/galeria-institucional/
            new GaleriaImage { Url = "/img/galeria-institucional/1.jpg", ThumbnailUrl = "/img/galeria-institucional/1.jpg", Title = "Ceremonia de Ascensos", AltText = "Jefe de policia" },
            new GaleriaImage { Url = "/img/galeria-institucional/2.jpg", ThumbnailUrl = "/img/galeria-institucional/2.jpg", Title = "Nuevo Equipamiento", AltText = "Nuevos patrulleros de la Policía de Entre Ríos" },
            new GaleriaImage { Url = "/img/galeria-institucional/3.jpg", ThumbnailUrl = "/img/galeria-institucional/3.jpg", Title = "Capacitación Táctica", AltText = "Grupo especial en entrenamiento" },
            new GaleriaImage { Url = "/img/galeria-institucional/4.jpg", ThumbnailUrl = "/img/galeria-institucional/4.jpg", Title = "División Montada", AltText = "Agente de la división montada patrullando" },
            new GaleriaImage { Url = "/img/galeria-institucional/5.jpg", ThumbnailUrl = "/img/galeria-institucional/5.jpg", Title = "Comando Radioeléctrico", AltText = "Operador en el comando radioeléctrico" },
            new GaleriaImage { Url = "/img/galeria-institucional/6.jpg", ThumbnailUrl = "/img/galeria-institucional/6.jpg", Title = "Aniversario de la Institución", AltText = "Desfile por el aniversario de la Policía" },
            
            // Puedes agregar muchas más imágenes aquí...
        };

        // Esta acción simplemente devuelve la página principal de "Institucional"
        public IActionResult Index()
        {
            return View();
        }

        // Este es nuestro nuevo endpoint de API para la galería
        [HttpGet]
        public async Task<IActionResult> GetGaleriaImages(int page = 1, int pageSize = 6)
        {
            // Simula una pequeña espera, como si fuera una base de datos real.
            // Esto ayuda a ver el ícono de "cargando" cuando la red es rápida.
            await Task.Delay(500);

            // Usa LINQ para paginar los resultados: salta las páginas anteriores y toma el tamaño del lote actual.
            var images = _allImages
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Devuelve las imágenes encontradas en formato JSON
            return new JsonResult(images);
        }
    }
}