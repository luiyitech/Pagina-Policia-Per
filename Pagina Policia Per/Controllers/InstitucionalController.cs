using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pagina_Policia_Per.Controllers
{
    public class InstitucionalController : Controller
    {
        // Esta lista es solo para tu galería y está perfecta.
        // La sintaxis 'new()' es una forma más limpia que recomienda Visual Studio.
        private static readonly List<GaleriaImage> _allImages = new()
        {
            new() { Url = "/img/galeria-institucional/1.jpg", ThumbnailUrl = "/img/galeria-institucional/1.jpg", Title = "Ceremonia de Ascensos", AltText = "Jefe de policia" },
            new() { Url = "/img/galeria-institucional/2.jpg", ThumbnailUrl = "/img/galeria-institucional/2.jpg", Title = "Nuevo Equipamiento", AltText = "Nuevos patrulleros de la Policía de Entre Ríos" },
            new() { Url = "/img/galeria-institucional/3.jpg", ThumbnailUrl = "/img/galeria-institucional/3.jpg", Title = "Capacitación Táctica", AltText = "Grupo especial en entrenamiento" },
            new() { Url = "/img/galeria-institucional/4.jpg", ThumbnailUrl = "/img/galeria-institucional/4.jpg", Title = "División Montada", AltText = "Agente de la división montada patrullando" },
            new() { Url = "/img/galeria-institucional/5.jpg", ThumbnailUrl = "/img/galeria-institucional/5.jpg", Title = "Comando Radioeléctrico", AltText = "Operador en el comando radioeléctrico" },
            new() { Url = "/img/galeria-institucional/6.jpg", ThumbnailUrl = "/img/galeria-institucional/6.jpg", Title = "Aniversario de la Institución", AltText = "Desfile por el aniversario de la Policía" },
        };

        // Esta es la acción Index correcta.
        // No envía ningún modelo a la vista, porque la vista es estática.
        public IActionResult Index()
        {
            return View();
        }

        // Tu API para la galería no necesita cambios y funcionará perfectamente.
        [HttpGet]
        public async Task<IActionResult> GetGaleriaImages(int page = 1, int pageSize = 6)
        {
            await Task.Delay(500);

            var images = _allImages
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new JsonResult(images);
        }
    }
}