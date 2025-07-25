// Archivo: Models/NoticiaCreateModel.cs

using Microsoft.AspNetCore.Http; // Necesario para IFormFile
using System.ComponentModel.DataAnnotations;

namespace Pagina_Policia_Per.Models
{
    public class NoticiaCreateModel
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede tener más de 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El resumen es obligatorio.")]
        [StringLength(250, ErrorMessage = "El resumen no puede tener más de 250 caracteres.")]
        public string Resumen { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio.")]
        public string Contenido { get; set; }

        // Comentamos o eliminamos la propiedad antigua que ya no usamos.
        // public string ImagenUrl { get; set; }

        // ¡AÑADIMOS LA NUEVA PROPIEDAD PARA EL ARCHIVO!
        // Ahora, cuando la vista busque "ImagenPrincipal", la encontrará.
        [Required(ErrorMessage = "La imagen principal es obligatoria.")]
        [Display(Name = "Imagen Principal")] // Esto mejora el texto que se muestra en la etiqueta <label>
        public IFormFile ImagenPrincipal { get; set; }
    }
}