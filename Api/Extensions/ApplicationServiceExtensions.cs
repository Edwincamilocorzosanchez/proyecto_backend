using FluentValidation;
using MediatR;
using Application.Abstractions;
using Infrastructure.UnitOfWork;
using System.Threading.RateLimiting;
using Api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Api.Helpers.Errors;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Auth;
using Api.Services.Implementations;
using Api.Services.Interfaces;
using Api.Services.Interfaces.Auth;
using Api.Services.Implementations.Auth;
namespace Api.Extensions;

// este archivo define ciertos metodos de extensión para la aplicación, como CORS, JWT, servicios de aplicacion, RateLimiter, errores de validación, etc...
public static class ApplicationServiceExtensions
{
    // este es el metodo de CORS que se usa en la aplicación
    public static void ConfigureCors(this IServiceCollection services) =>

        services.AddCors(options =>
        {
            // estos son los dominios permitidos para la aplicación
            HashSet<String> allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "localhost:5173",
                "localhost:5500",
                "localhost:5501",
                "localhost:5001",
                "localhost:4200",
                "localhost:8080",
                "localhost:8081",
            };
            // este es el comportamiento de CORS
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()   //WithOrigins("https://dominio.com")
                .AllowAnyMethod()          //WithMethods("GET","POST")
                .AllowAnyHeader());        //WithHeaders("accept","content-type")

            // este es el comportamiento de CORS para URLs específicas
            options.AddPolicy("CorsPolicyUrl", builder =>
                builder.WithOrigins("https://localhost:4200", "https://localhost:5500")   //WithOrigins("https://dominio.com")
                .AllowAnyMethod()          //WithMethods("GET","POST")
                .AllowAnyHeader());
            // otro comportamiento de CORS
            options.AddPolicy("Dinamica", builder =>
                builder.SetIsOriginAllowed(origin => allowed.Contains(origin))   //WithOrigins("https://dominio.com")
                .WithMethods("GET", "POST")
                .WithHeaders("Content-Type", "Authorization"));        //WithHeaders("accept","content-type")
        });
    // este método registra los servicios de la aplicación
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // PasswordHasher
        services.AddScoped<IPasswordHasher<UserMember>, PasswordHasher<UserMember>>();
        // Servicios de dominio / aplicación
        services.AddScoped<IAdministradorService, AdministradorService>();
        services.AddScoped<ICitaService, CitaService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IDetalleOrdenService, DetalleOrdenService>();
        services.AddScoped<IEstadoCitaService, EstadoCitaService>();
        services.AddScoped<IEstadoPagoService, EstadoPagoService>();
        services.AddScoped<IEstadoOrdenService, EstadoOrdenService>();
        services.AddScoped<IFacturaService, FacturaService>();
        services.AddScoped<IHistorialInventarioService, HistorialInventarioService>();
        services.AddScoped<IMecanicoService, MecanicoService>();
        services.AddScoped<IMetodoPagoService, MetodoPagoService>();
        services.AddScoped<IOrdenServicioService, OrdenServicioService>();
        services.AddScoped<IPagoService, PagoService>();
        services.AddScoped<IProveedorService, ProveedorService>();
        services.AddScoped<IRepuestoService, RepuestoService>();
        services.AddScoped<ITipoServicioService, TipoServicioService>();
        services.AddScoped<ITipoMovimientoService, TipoMovimientoService>();
        services.AddScoped<IVehiculoService, VehiculoService>();
        services.AddScoped<IUserService, UserService>();

        // esto es para agregar los servicios de persistencia
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // esto es para agregar los servicios de MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        // AutoMapper: detecta automáticamente todos los perfiles de todos los proyectos
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
    // este es el meetodo para agregar el RateLimiter 
    public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, token) =>
            {
                var role = context.HttpContext.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value ?? "unknown";
                var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var timestamp = DateTime.UtcNow.ToString("o");
                context.HttpContext.Response.StatusCode = 429;
                context.HttpContext.Response.ContentType = "application/json";
                var mensaje = $"{{\"message\": \"Demasiadas peticiones. Rol: {role}, IP: {ip}\", \"timestamp\": \"{timestamp}\"}}";
                await context.HttpContext.Response.WriteAsync(mensaje, token);
            };

            // Aquí no se define GlobalLimiter
            options.AddPolicy("ipLimiter", httpContext =>
            {
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    Window = TimeSpan.FromSeconds(10),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });

            // Rate Limiter basado en rol JWT
            options.AddPolicy("roleBasedLimiter", httpContext =>
            {
                var role = httpContext.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                // Si no hay rol (token inválido o anónimo), usar IP como clave
                var partitionKey = role ?? ip;

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = GetLimitForRole(role, httpContext.Request.Method, httpContext.Request.Path),
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });
        });
        return services;
    }

    // Método auxiliar para determinar el límite basado en rol y ruta
    private static int GetLimitForRole(string? role, string method, string path)
    {
        // Rutas de escritura sensibles
        var writePaths = new[] { "/api/ordenesservicio", "/api/repuestos" };
        var isWrite = new[] { "POST", "PUT", "DELETE" }.Contains(method.ToUpper());
        var isSensitivePath = writePaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase));

        if (isWrite && isSensitivePath)
        {
            return role switch
            {
                "Admin" => 20,
                "Recepcionista" => 5,
                _ => 5 // Fallback para IP o roles desconocidos
            };
        }

        // Rutas de lectura (GET)
        if (method.ToUpper() == "GET")
        {
            return 50; // Límite común para lecturas
        }

        // Otros casos
        return 10;
    }
    // este es el metodo para agregar el JWT
    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        //Configuration from AppSettings
        services.Configure<JWT>(configuration.GetSection("JWT"));

        //Adding Athentication - JWT
        _ = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                };
            });
        // 3. Authorization – Policies
        services.AddAuthorization(options =>
        {
            // Política que exige rol Admin
            options.AddPolicy("Admins", policy =>
                policy.RequireRole("Administrador"));

            options.AddPolicy("Clientes", policy =>
                policy.RequireRole("Cliente"));

            options.AddPolicy("Pro", policy =>
                policy.RequireRole("Professional"));

            // Política que exige claim Subscription = "Premium"
            options.AddPolicy("Professional", policy =>
                policy.RequireClaim("Subscription", "Premium"));

            // Política compuesta: rol Admin o claim Premium
            options.AddPolicy("OtherOPremium", policy =>
                policy.RequireAssertion(context =>
                    context.User.IsInRole("Other")
                || context.User.HasClaim(c =>
                        c.Type == "Subscription" && c.Value == "Premium")));
        });
    }
    // este metodos sirve para agregar los errores de validacion
    public static void AddValidationErrors(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {

                var errors = actionContext.ModelState.Where(u => u.Value!.Errors.Count > 0)
                                                .SelectMany(u => u.Value!.Errors)
                                                .Select(u => u.ErrorMessage).ToArray();

                var errorResponse = new ApiValidation()
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });
    }
}
