using GestionEventosBack.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// ---- SWAGGER ----
// Genera la documentación de la API para probarla en el navegador
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//AGREGAR NUGET Swashbuckle.AspNetCore
// ---- BASE DE DATOS ----
// Le dice a la aplicación cómo conectarse a SQL Server
// Usa la cadena de conexión "LocalConnection" del appsettings.json
builder.Services.AddDbContext<DataContext>(x =>
    x.UseSqlServer("name=LocalConnection"));

// ---- CORS ----
// Permite que el Front (Blazor) pueda comunicarse con esta API
// Sin esto el navegador bloquea las peticiones entre proyectos
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true)
              .AllowCredentials();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// ---- SWAGGER EN DESARROLLO ----
// Solo muestra la interfaz de Swagger cuando estamos desarrollando
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();

app.Run();
