using Api.Helpers;
using Domain.Entities.Auth;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

// este archivo se encarga de sermbrar datos iniciales en la base de datos de la aplicación (roles, usuarios, etc...) todo sin la necesidad de crearlos manualmente
public static class DbSeederExtensions
{
    public static async Task SeedRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var existingNames = await db.Roles.Select(r => r.Name).ToListAsync();
        var targetNames = Enum.GetNames(typeof(UserAuthorization.Roles));

        var toAdd = targetNames
            .Except(existingNames, StringComparer.OrdinalIgnoreCase)
            .Select(n => new Rol
            {
                Name = n,
                Description = $"{n} role"
            })
            .ToList();

        if (toAdd.Count > 0)
        {
            db.Roles.AddRange(toAdd);
            await db.SaveChangesAsync();
        }
    }
    // TODO: agregar métodos para agregar datos iniciales a la base de datos
    
}
