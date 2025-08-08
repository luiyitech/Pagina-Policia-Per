using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pagina_Policia_Per.Data; // Aseg�rate de que este namespace coincida con la ubicaci�n de tu ApplicationDbContext
using Microsoft.Extensions.DependencyInjection; // Necesario para GetRequiredService y CreateScope
using Microsoft.Extensions.Logging; // Necesario para ILogger
using System; // Necesario para Environment

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Esta secci�n es donde agregas la mayor�a de tus servicios
builder.Services.AddControllersWithViews();

// ==================================================================
// COMIENZO de la configuraci�n de Identity y EF Core
// Esto DEBE estar dentro del bloque donde se configuran los servicios (builder.Services)
// ==================================================================

// Configura Entity Framework Core para usar SQL Server (para tiempo de dise�o y cuando uses SQL Server real)
// !!! IMPORTANTE: Para ejecutar la aplicaci�n con base de datos en memoria (modo prueba), COMENTA estas l�neas.
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(connectionString)); // Configuraci�n para usar SQL Server

// Configura Entity Framework Core para usar la base de datos en memoria (para pruebas)
// !!! IMPORTANTE: Para ejecutar la aplicaci�n con SQL Server, COMENTA esta l�nea y DESCOMENTA las anteriores.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("IdentityDatabase")); // <--- DESCOMENTAMOS ESTA L�NEA PARA EL MODO PRUEBA
                                                      // Nombre de la base de datos en memoria para pruebas


// Configura Identity para usar IdentityUser y IdentityRole con Entity Framework Core
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configura la ruta para la p�gina de inicio de sesi�n
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// ==================================================================
// FIN de la configuraci�n de Identity y EF Core
// ==================================================================


var app = builder.Build();

// ==================================================================
// COMIENZO: C�digo de inicializaci�n para crear Roles y Usuario Administrador (Solo para desarrollo/pruebas)
// Este bloque se ejecuta al inicio de la aplicaci�n.
// IMPORTANTE: Si cambias entre base de datos en memoria y SQL Server,
// este c�digo se ejecutar� cada vez que inicies la aplicaci�n.
// Considera c�mo quieres manejar la creaci�n de usuarios/roles en la base de datos real.
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
        // Usamos GetAwaiter().GetResult() porque este c�digo se ejecuta en un bloque s�ncrono
        // aunque las operaciones de Identity/EF Core son as�ncronas.
        // Es una pr�ctica com�n en c�digo de inicializaci�n s�ncrono que necesita llamar a m�todos as�ncronos.
        if (!roleManager.RoleExistsAsync(adminRoleName).GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole(adminRoleName)).GetAwaiter().GetResult();
        }

        // Crea un usuario administrador si no existe
        const string adminEmail = "admin@tudominio.com"; // !!! CAMBIA este correo
        const string adminPassword = "LuiyiTech123!"; // !!! CAMBIA esta contrase�a a algo SEGURO

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
        logger.LogError(ex, "Ocurri� un error al inicializar la base de datos Identity.");
    }
}
// ==================================================================
// FIN: C�digo de inicializaci�n para crear Roles y Usuario Administrador
// ==================================================================


// Configure the HTTP request pipeline.
// Esta secci�n es donde configuras el pipeline de solicitud (middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ==================================================================
// COMIENZO: Aseg�rate de que Authentication y Authorization est�n en el orden correcto
// Esto DEBE estar dentro del bloque donde se configura el pipeline (app.Use...)
// ==================================================================
app.UseAuthentication(); // Habilita la autenticaci�n
app.UseAuthorization();  // Habilita la autorizaci�n
// ==================================================================
// FIN: Aseg�rate de que Authentication y Authorization est�n en el orden correcto
// ==================================================================


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
