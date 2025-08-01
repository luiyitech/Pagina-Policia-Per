namespace Pagina_Policia_Per.Models
{
    public class Noticia
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Resumen { get; set; }
        public string? Contenido { get; set; }
        public string? ImagenUrl { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string? Slug { get; set; } // URL amigable
    }
}