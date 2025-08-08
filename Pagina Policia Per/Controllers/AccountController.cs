using Microsoft.AspNetCore.Mvc;
using Pagina_Policia_Per.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; // Necesario para UserManager y SignInManager
using Microsoft.AspNetCore.Authorization; // Necesario para [AllowAnonymous]

namespace Pagina_Policia_Per.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        // Inyectamos UserManager y SignInManager en el constructor
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Acción para mostrar la página de inicio de sesión
        [HttpGet]
        [AllowAnonymous] // Permite que usuarios no autenticados accedan a esta acción
        public IActionResult Login(string returnUrl = null) // Añadimos returnUrl para redirigir al usuario a la página que intentó acceder antes de iniciar sesión
        {
            // Almacena la URL de retorno en ViewData para usarla en la vista
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Acción para procesar el formulario de inicio de sesión
        [HttpPost]
        [AllowAnonymous] // Permite que usuarios no autenticados envíen este formulario
        [ValidateAntiForgeryToken] // Importante para seguridad
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            // Almacena la URL de retorno en ViewData si el modelo no es válido y volvemos a la vista
            ViewData["ReturnUrl"] = returnUrl;

            // Verifica si el modelo tiene errores de validación (basado en los atributos en LoginViewModel)
            if (ModelState.IsValid)
            {
                // Intenta iniciar sesión con el correo/nombre de usuario y contraseña
                var result = await _signInManager.PasswordSignInAsync(model.EmailOrUserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                // Si el inicio de sesión fue exitoso
                if (result.Succeeded)
                {
                    // Opcional: Loggear el inicio de sesión exitoso
                    // _logger.LogInformation("Usuario inició sesión.");

                    // Redirige al usuario a la URL original que intentó acceder (si existe)
                    // o a la página principal si no hay URL de retorno.
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        // Redirige a la página de inicio por defecto después de iniciar sesión
                        return RedirectToAction("Index", "Home"); // Cambia "Home" y "Index" si tu página de inicio es diferente
                    }
                }

                // Si el inicio de sesión falló (credenciales incorrectas, etc.)
                // Opcional: Loggear el intento de inicio de sesión fallido
                // _logger.LogWarning("Intento de inicio de sesión inválido.");
                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión inválido."); // Agrega un mensaje de error al resumen de validación
            }

            // Si el modelo no es válido o el inicio de sesión falló, vuelve a mostrar la vista con los errores
            return View(model);
        }

        // Acción de cierre de sesión
        [HttpPost]
        [Authorize] // Solo usuarios autenticados pueden cerrar sesión
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Cierra la sesión del usuario
            // Opcional: Loggear el cierre de sesión
            // _logger.LogInformation("Usuario cerró sesión.");
            return RedirectToAction("Index", "Home"); // Redirige a la página de inicio después de cerrar sesión
        }


        // Acción para mostrar la página de acceso denegado
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
