using Api;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// esto es para poder colocar los servicios 
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi(); se va a usar con Swagger por lo que no se usa
// se usa esto en su lugar 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("Postgres")!;
    options.UseNpgsql(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// builder.Services.AddDbContext<AppDbContext>(opt =>
//     opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }
// esto es para poder usar swagger


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanShop API v1");
        c.RoutePrefix = string.Empty; // opcional → Swagger en la raíz "/"
    });
}

// aqui se usa la ultima a menos que se espeficique que politica se va a usar en cada endpoint
// app.UseCors("CorsPolicy");
// app.UseCors("CorsPolicyUrl");
// app.UseCors("Dinamica");

app.UseHttpsRedirection();
// app.UseRateLimiter();
// JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
