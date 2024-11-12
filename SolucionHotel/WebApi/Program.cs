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

// Inyecciones de capa de negocio
builder.Services.AddTransient<IBitacoraLN, BitacoraLN>();
builder.Services.AddTransient<IUsuarioLN, UsuarioLN>();
builder.Services.AddTransient<IHabitacionLN, HabitacionLN>();
builder.Services.AddTransient<IReservacionLN, ReservacionLN>();

builder.Services.AddSingleton(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();