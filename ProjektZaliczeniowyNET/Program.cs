using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProjektZaliczeniowyNET.Data;
using ProjektZaliczeniowyNET.Interfaces;
using ProjektZaliczeniowyNET.Models;
using ProjektZaliczeniowyNET.Services;
using ProjektZaliczeniowyNET.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja bazy danych
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfiguracja Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Konfiguracja hasła
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;

        // Konfiguracja użytkownika
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// domyslna sciezka do widoku braku dostepu
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/AccessDenied"; // ścieżka do Twojej akcji AccessDenied w HomeController
    options.LoginPath = "/Account/Login"; // (opcjonalnie) ścieżka do logowania
});

// Rejestracja serwisów

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<VehicleMapper>();

builder.Services.AddScoped<IServiceOrderService, ServiceOrderService>();
builder.Services.AddScoped<ServiceOrderMapper>();

builder.Services.AddScoped<IServiceOrderPartService, ServiceOrderPartService>();
builder.Services.AddScoped<ServiceOrderPartMapper>();

builder.Services.AddScoped<IServiceTaskService, ServiceTaskService>();
builder.Services.AddScoped<ServiceTaskMapper>();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<CustomerMapper>();

builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<CommentMapper>();

builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<PartMapper>();  

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// seedowanie
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // role
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "Mechanik", "Recepcjonista" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
    
    // czesci
    var context = services.GetRequiredService<ApplicationDbContext>(); 
    if (!context.Parts.Any())
    {
        var parts = new List<Part>
        {
            new Part { Name = "Niestandardowa", UnitPrice = 0m, QuantityInStock = 100 },
        };
        context.Parts.AddRange(parts);
        await context.SaveChangesAsync();
    }
    
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Dodane dla plików statycznych
app.UseRouting();

app.UseAuthentication(); // WAŻNE: przed UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();