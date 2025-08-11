using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pagina_Policia_Per.Data; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Logging;
using System; 

var builder = WebApplication.CreateBuilder(args);

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


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Crea el rol "Admin" si no existe
        const string adminRoleName = "Admin";
               if (!roleManager.RoleExistsAsync(adminRoleName).GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole(adminRoleName)).GetAwaiter().GetResult();
        }

        // Crea un usuario administrador si no existe
        const string adminEmail = "admin@tudominio.com";
        const string adminPassword = "LuiyiTech123!";

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


// Pipeline de solicitud (middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication(); // Habilita la autenticación
app.UseAuthorization();  // Habilita la autorización


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
