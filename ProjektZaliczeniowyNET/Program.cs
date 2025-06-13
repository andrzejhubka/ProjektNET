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
builder.Services.AddIdentity<User, ApplicationRole>(options =>
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

// Rejestracja serwisów
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserMapper>();

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

builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
builder.Services.AddScoped<ApplicationRoleMapper>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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