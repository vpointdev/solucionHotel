using AccesoDatos;
using AccesoDatos.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Negocio;
using Negocio.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Vinculación del archivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Inyecciones de capa de acceso a datos
builder.Services.AddTransient<IBitacoraAD, BitacoraAD>();
builder.Services.AddTransient<IUsuarioAD, UsuarioAD>();
builder.Services.AddTransient<IHabitacionAD, HabitacionAD>();
builder.Services.AddTransient<IReservacionAD, ReservacionAD>();
builder.Services.AddTransient<IPagoAD, PagoAD>();

// Inyecciones de capa de negocio
builder.Services.AddTransient<IBitacoraLN, BitacoraLN>();
builder.Services.AddTransient<IUsuarioLN, UsuarioLN>();
builder.Services.AddTransient<IHabitacionLN, HabitacionLN>();
builder.Services.AddTransient<IReservacionLN, ReservacionLN>();
builder.Services.AddTransient<IPagoLN, PagoLN>();

builder.Services.AddSingleton(builder.Configuration.GetSection("ConnectionStrings"));

// Change this line to support both API and Views
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add these middleware components for MVC to work properly
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Add both MVC and API routing
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();