using Microsoft.EntityFrameworkCore;
using FicticiaApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios para controladores
builder.Services.AddControllers();

// Agregar Swagger para documentación (opcional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Iniciar la aplicación
app.Run();
