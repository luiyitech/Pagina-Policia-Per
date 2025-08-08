using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pagina_Policia_Per.Data; // Asegúrate de que este namespace coincida con la ubicación de tu ApplicationDbContext
using Microsoft.Extensions.DependencyInjection; // Necesario para GetRequiredService y CreateScope
using Microsoft.Extensions.Logging; // Necesario para ILogger
using System; // Necesario para Environment

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Esta sección es donde agregas la mayoría de tus servicios
builder.Services.AddControllersWithViews();

// ==================================================================
// COMIENZO de la configuración de Identity y EF Core
// Esto DEBE estar dentro del bloque donde se configuran los servicios (builder.Services)
// ==================================================================

// Configura Entity Framework Core para usar SQL Server (para tiempo de diseño y cuando uses SQL Server real)
// !!! IMPORTANTE: Para ejecutar la aplicación con base de datos en memoria (modo prueba), COMENTA estas líneas.
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(connectionString)); // Configuración para usar SQL Server

// Configura Entity Framework Core para usar la base de datos en memoria (para pruebas)
// !!! IMPORTANTE: Para ejecutar la aplicación con SQL Server, COMENTA esta línea y DESCOMENTA las anteriores.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("IdentityDatabase")); // <--- DESCOMENTAMOS ESTA LÍNEA PARA EL MODO PRUEBA
                                                      // Nombre de la base de datos en memoria para pruebas


// Configura Identity para usar IdentityUser y IdentityRole con Entity Framework Core
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configura la ruta para la página de inicio de sesión
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// ==================================================================
// FIN de la configuración de Identity y EF Core
// ==================================================================


var app = builder.Build();

// ==================================================================
// COMIENZO: Código de inicialización para crear Roles y Usuario Administrador (Solo para desarrollo/pruebas)
// Este bloque se ejecuta al inicio de la aplicación.
// IMPORTANTE: Si cambias entre base de datos en memoria y SQL Server,
// este código se ejecutará cada vez que inicies la aplicación.
// Considera cómo quieres manejar la creación de usuarios/roles en la base de datos real.
// ==================================================================
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Crea el rol "Admin" si no existe
        const string adminRoleName = "Admin";
        // Usamos GetAwaiter().GetResult() porque este código se ejecuta en un bloque síncrono
        // aunque las operaciones de Identity/EF Core son asíncronas.
        // Es una práctica común en código de inicialización síncrono que necesita llamar a métodos asíncronos.
        if (!roleManager.RoleExistsAsync(adminRoleName).GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole(adminRoleName)).GetAwaiter().GetResult();
        }

        // Crea un usuario administrador si no existe
        const string adminEmail = "admin@tudominio.com"; // !!! CAMBIA este correo
        const string adminPassword = "LuiyiTech123!"; // !!! CAMBIA esta contraseña a algo SEGURO

        var adminUser = userManager.FindByNameAsync(adminEmail).GetAwaiter().GetResult();
        if (adminUser == null)
        {
            var user = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, adminPassword).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, adminRoleName).GetAwaiter().GetResult();
            }
            else
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                foreach (var error in result.Errors)
                {
                    logger.LogError($"Error creando usuario administrador: {error.Description}");
                }
            }
        }
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar la base de datos Identity.");
    }
}
// ==================================================================
// FIN: Código de inicialización para crear Roles y Usuario Administrador
// ==================================================================


// Configure the HTTP request pipeline.
// Esta sección es donde configuras el pipeline de solicitud (middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ==================================================================
// COMIENZO: Asegúrate de que Authentication y Authorization estén en el orden correcto
// Esto DEBE estar dentro del bloque donde se configura el pipeline (app.Use...)
// ==================================================================
app.UseAuthentication(); // Habilita la autenticación
app.UseAuthorization();  // Habilita la autorización
// ==================================================================
// FIN: Asegúrate de que Authentication y Authorization estén en el orden correcto
// ==================================================================


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
