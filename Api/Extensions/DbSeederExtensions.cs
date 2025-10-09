using Api.Helpers;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;

public static class DbSeederExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await SeedRolesAsync(db);
        await SeedUsersAsync(db);
        await SeedEstadosAsync(db);
        await SeedTiposMovimientoAsync(db);
        await SeedMetodosPagoAsync(db);
        await SeedTiposServicioAsync(db);
        await SeedProveedoresAsync(db);
        await SeedRepuestosAsync(db);

        // Si quieres, aquí puedes agregar más métodos para clientes, mecanicos, etc.
    }

    // =========================================
    // Roles
    // =========================================
    private static async Task SeedRolesAsync(AppDbContext db)
    {
        var existing = await db.Roles.Select(r => r.Name).ToListAsync();
        var targetRoles = Enum.GetNames(typeof(UserAuthorization.Roles));

        var toAdd = targetRoles
            .Except(existing, StringComparer.OrdinalIgnoreCase)
            .Select(n => new Rol
            {
                Name = n,
                Description = $"{n} role"
            })
            .ToList();

        if (toAdd.Any())
        {
            db.Roles.AddRange(toAdd);
            await db.SaveChangesAsync();
        }
    }

    // =========================================
    // Usuarios de prueba
    // =========================================
    private static async Task SeedUsersAsync(AppDbContext db)
    {
        if (await db.UsersMembers.AnyAsync()) return;

        var users = new List<UserMember>
        {
            new UserMember { Username = "admin", Email = "admin@example.com", Password = "1234" },
            new UserMember { Username = "mecanico1", Email = "mecanico1@example.com", Password = "1234" },
            new UserMember { Username = "cliente1", Email = "cliente1@example.com", Password = "1234" },
        };

        db.UsersMembers.AddRange(users);
        await db.SaveChangesAsync();

        // Asignar roles
        var adminRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        var mecanicoRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "Mecanico");
        var clienteRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "Cliente");

        if (adminRole != null)
            db.UserMemberRols.Add(new UserMemberRol { UserMemberId = users[0].Id, RolId = adminRole.Id });
        if (mecanicoRole != null)
            db.UserMemberRols.Add(new UserMemberRol { UserMemberId = users[1].Id, RolId = mecanicoRole.Id });
        if (clienteRole != null)
            db.UserMemberRols.Add(new UserMemberRol { UserMemberId = users[2].Id, RolId = clienteRole.Id });

        await db.SaveChangesAsync();
    }

    // =========================================
    // Estados de cita y orden
    // =========================================
    private static async Task SeedEstadosAsync(AppDbContext db)
    {
        // Estados de cita
        var estadosCita = new[] { "Pendiente", "Confirmada", "Cancelada", "Completada" };
        var existingCita = await db.EstadosCita.Select(e => e.Nombre).ToListAsync();
        db.EstadosCita.AddRange(
            estadosCita.Except(existingCita)
                       .Select(e => new EstadoCita { Nombre = e })
        );

        // Estados de orden
        var estadosOrden = new[] { "Pendiente", "En Proceso", "Finalizada", "Cancelada" };
        var existingOrden = await db.EstadosOrden.Select(e => e.Nombre).ToListAsync();
        db.EstadosOrden.AddRange(
            estadosOrden.Except(existingOrden)
                        .Select(e => new EstadoOrden { Nombre = e })
        );

        await db.SaveChangesAsync();
    }

    // =========================================
    // Tipos de movimiento
    // =========================================
    private static async Task SeedTiposMovimientoAsync(AppDbContext db)
    {
        var movimientos = new[] { "Ingreso", "Salida", "Ajuste" };
        var existing = await db.TiposMovimiento.Select(t => t.Nombre).ToListAsync();

        db.TiposMovimiento.AddRange(
            movimientos.Except(existing).Select(m => new TipoMovimiento { Nombre = m })
        );

        await db.SaveChangesAsync();
    }

    // =========================================
    // Métodos de pago
    // =========================================
    private static async Task SeedMetodosPagoAsync(AppDbContext db)
    {
        var metodos = new[] { "Efectivo", "Tarjeta", "Transferencia" };
        var existing = await db.MetodosPago.Select(m => m.Nombre).ToListAsync();

        db.MetodosPago.AddRange(
            metodos.Except(existing).Select(m => new MetodoPago { Nombre = m })
        );

        await db.SaveChangesAsync();
    }

    // =========================================
    // Tipos de servicio
    // =========================================
    private static async Task SeedTiposServicioAsync(AppDbContext db)
    {
        if (!await db.TiposServicio.AnyAsync())
        {
            var servicios = new List<TipoServicio>
            {
                new TipoServicio { Nombre = "Cambio de aceite", Descripcion = "Cambio de aceite y filtro", PrecioBase = 50 },
                new TipoServicio { Nombre = "Frenos", Descripcion = "Revisión y cambio de frenos", PrecioBase = 80 },
                new TipoServicio { Nombre = "Diagnóstico", Descripcion = "Diagnóstico completo del vehículo", PrecioBase = 100 },
            };
            db.TiposServicio.AddRange(servicios);
            await db.SaveChangesAsync();
        }
    }

    // =========================================
    // Proveedores de prueba
    // =========================================
    private static async Task SeedProveedoresAsync(AppDbContext db)
    {
        if (!await db.Proveedores.AnyAsync())
        {
            var proveedores = new List<Proveedor>
            {
                new Proveedor { Nombre = "Proveedor A", Telefono = "123456789", Correo = "provA@example.com", Direccion = "Calle 1" },
                new Proveedor { Nombre = "Proveedor B", Telefono = "987654321", Correo = "provB@example.com", Direccion = "Calle 2" }
            };
            db.Proveedores.AddRange(proveedores);
            await db.SaveChangesAsync();
        }
    }

    // =========================================
    // Repuestos de prueba
    // =========================================
    private static async Task SeedRepuestosAsync(AppDbContext db)
    {
        if (!await db.Repuestos.AnyAsync())
        {
            var proveedor = await db.Proveedores.FirstOrDefaultAsync();
            var repuestos = new List<Repuesto>
            {
                new Repuesto { Codigo = "REP001", Descripcion = "Filtro de aceite", CantidadStock = 20, PrecioUnitario = 15, ProveedorId = proveedor?.Id },
                new Repuesto { Codigo = "REP002", Descripcion = "Pastillas de freno", CantidadStock = 50, PrecioUnitario = 30, ProveedorId = proveedor?.Id },
                new Repuesto { Codigo = "REP003", Descripcion = "Batería 12V", CantidadStock = 10, PrecioUnitario = 120, ProveedorId = proveedor?.Id }
            };
            db.Repuestos.AddRange(repuestos);
            await db.SaveChangesAsync();
        }
    }
}
