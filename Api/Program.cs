using Api.Extensions;
using Api.Mappings;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// REGISTRA SERVICIOS Y CONFIGURACIONES PERSONALIZADAS DEL APPLICATIONSERVICEEXTENSION 
builder.Services.ConfigureCors();
builder.Services.AddApplicationServices();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddValidationErrors();

// Configurar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    string connectionString = builder.Configuration.GetConnectionString(isDocker ? "PostgresDocker" : "PostgresLocal")!;
    options.UseNpgsql(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

var app = builder.Build();
// muestra la conexion a la base de datos de Postgres
Console.WriteLine(builder.Configuration.GetConnectionString("Postgres"));

// Swagger y middlewares
if (app.Environment.IsDevelopment())
{
    // agregar traces completos y errores detallador
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proyecto full-stack");
        // esto hace que swagger se ejecute en la raiz
        c.RoutePrefix = string.Empty; 
    });
}

// esto es para agregar los Seeders de la base de datos
await app.SeedDatabaseAsync();

// agregar CORS y RateLimiter
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
// app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
