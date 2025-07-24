using Pagina_Policia_Per.Services; // ¡Añadimos el 'using' para nuestro nuevo servicio!

var builder = WebApplication.CreateBuilder(args);

// --- SECCIÓN DE CONFIGURACIÓN DE SERVICIOS ---
// Aquí le decimos a la aplicación qué "herramientas" están disponibles.

// 1. Añade los servicios básicos para que funcione MVC (Controladores y Vistas).
builder.Services.AddControllersWithViews();

// 2. ¡LÍNEA CLAVE! Registramos nuestro NoticiaService como un "Singleton".
//    Esto significa que habrá una única instancia de NoticiaService compartida
//    en toda la aplicación, asegurando que todos los controladores
//    accedan a la misma lista de noticias.
builder.Services.AddSingleton<NoticiaService>();


// --- CONSTRUCCIÓN DE LA APLICACIÓN ---
var app = builder.Build();


// --- CONFIGURACIÓN DEL PIPELINE DE PETICIONES HTTP ---
// Aquí definimos cómo la aplicación responderá a las peticiones del navegador.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // En producción, usa una página de error amigable.
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirige las peticiones HTTP a HTTPS.
app.UseHttpsRedirection();

// Permite que la aplicación sirva archivos estáticos desde la carpeta wwwroot (CSS, JS, imágenes).
app.UseStaticFiles();

// Habilita el sistema de rutas.
app.UseRouting();

// Habilita la autorización (no la estamos usando activamente, pero es buena práctica tenerla).
app.UseAuthorization();

// Define el patrón de ruta por defecto para MVC, que ya no usaremos mucho
// porque ahora preferimos el "Attribute Routing" en los controladores.
// Sin embargo, se mantiene por compatibilidad.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia la aplicación y la pone a la escucha de peticiones.
app.Run();