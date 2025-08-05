namespace Pagina_Policia_Per.Models
{
    public class Tramite
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }      // La '?' es importante
        public string? ImagenUrl { get; set; }   // La '?' es importante
        public string? UrlDestino { get; set; }  // La '?' es importante
    }
}