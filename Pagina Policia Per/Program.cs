using Pagina_Policia_Per.Services; // �A�adimos el 'using' para nuestro nuevo servicio!

var builder = WebApplication.CreateBuilder(args);

// --- SECCI�N DE CONFIGURACI�N DE SERVICIOS ---
// Aqu� le decimos a la aplicaci�n qu� "herramientas" est�n disponibles.

// 1. A�ade los servicios b�sicos para que funcione MVC (Controladores y Vistas).
builder.Services.AddControllersWithViews();

// 2. �L�NEA CLAVE! Registramos nuestro NoticiaService como un "Singleton".
//    Esto significa que habr� una �nica instancia de NoticiaService compartida
//    en toda la aplicaci�n, asegurando que todos los controladores
//    accedan a la misma lista de noticias.
builder.Services.AddSingleton<NoticiaService>();


// --- CONSTRUCCI�N DE LA APLICACI�N ---
var app = builder.Build();


// --- CONFIGURACI�N DEL PIPELINE DE PETICIONES HTTP ---
// Aqu� definimos c�mo la aplicaci�n responder� a las peticiones del navegador.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // En producci�n, usa una p�gina de error amigable.
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirige las peticiones HTTP a HTTPS.
app.UseHttpsRedirection();

// Permite que la aplicaci�n sirva archivos est�ticos desde la carpeta wwwroot (CSS, JS, im�genes).
app.UseStaticFiles();

// Habilita el sistema de rutas.
app.UseRouting();

// Habilita la autorizaci�n (no la estamos usando activamente, pero es buena pr�ctica tenerla).
app.UseAuthorization();

// Define el patr�n de ruta por defecto para MVC, que ya no usaremos mucho
// porque ahora preferimos el "Attribute Routing" en los controladores.
// Sin embargo, se mantiene por compatibilidad.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia la aplicaci�n y la pone a la escucha de peticiones.
app.Run();