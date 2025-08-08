using System.ComponentModel.DataAnnotations; // Necesario para los atributos de validación

namespace Pagina_Policia_Per.Models // Ajusta el namespace si creaste una subcarpeta
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico o nombre de usuario es obligatorio.")] // Indica que el campo es obligatorio
        [EmailAddress] // Opcional: si esperas un formato de correo electrónico
        [Display(Name = "Correo electrónico o Usuario")] // Etiqueta para la vista
        public string EmailOrUserName { get; set; } // Puedes usar "Email" si solo esperas correos

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)] // Para que el navegador oculte la entrada
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}
