// Archivo: Models/NoticiaCreateModel.cs

using System.ComponentModel.DataAnnotations; // Necesario para las anotaciones

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

        // Más adelante, este campo será para subir un archivo. Por ahora, es un texto para la URL.
        [Required(ErrorMessage = "La URL de la imagen es obligatoria.")]
        [Url(ErrorMessage = "Debe ser una URL válida.")]
        public string ImagenUrl { get; set; }
    }
}