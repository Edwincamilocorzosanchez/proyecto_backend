using Api.Helpers;
using Domain.Entities;
using Domain.Entities.Auth;
using Domain.ValueObjects;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Extensions;
public static class DbSeederExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.MigrateAsync();

        await SeedRolesAsync(db); // roles 
        await SeedBaseUsersAsync(db); // usuarios base
        await SeedExtendedUsersAsync(db); // extensiones de usuarios (Clientes, Proveedores, Mecanicos, Administradores)
        await SeedUserRolesAsync(db); // relación usuario - rol
        await SeedCatalogsAsync(db); // catálogos
        await SeedVehiculosAsync(db); // vehículos
        await SeedRepuestosAsync(db); // repuestos
        await SeedOrdenesYFacturasAsync(db); // ordenes de servicio y facturas
        await SeedHistorialInventarioAsync(db); // historial de inventario
    }
    // -------------------------------------------------------
    private static async Task SeedRolesAsync(AppDbContext db)
    {
        // Obtiene los nombres de roles ya existentes en la base de datos
        var existingNames = await db.Roles
            .Select(r => r.Name)
            .ToListAsync();

        // Obtiene los nombres de roles definidos en el enum
        var targetNames = Enum.GetNames(typeof(UserAuthorization.Roles));

        // Determina cuáles roles faltan por insertar
        var toAdd = targetNames
            .Except(existingNames, StringComparer.OrdinalIgnoreCase)
            .Select(name => new Rol
            {
                Name = name,
                Description = $"{name} role"
            })
            .ToList();

        // Inserta los roles faltantes
        if (toAdd.Count > 0)
        {
            db.Roles.AddRange(toAdd);
            await db.SaveChangesAsync();
        }
    }
    // -------------------------------------------------------
    private static async Task SeedBaseUsersAsync(AppDbContext db)
    {
        if (await db.UsersMembers.AnyAsync()) return;

        var users = new List<UserMember>
        {
            new UserMember { Username = "admin1", Email = "admin@taller.com", Password = "admin123" },
            new UserMember { Username = "cliente1", Email = "cliente@correo.com", Password = "cliente123" },
            new UserMember { Username = "mecanico1", Email = "mecanico@correo.com", Password = "mecanico123" },
            new UserMember { Username = "proveedor1", Email = "proveedor@correo.com", Password = "proveedor123" }
        };

        db.UsersMembers.AddRange(users);
        await db.SaveChangesAsync();
    }
    // -------------------------------------------------------
    private static async Task SeedExtendedUsersAsync(AppDbContext db)
    {
        var users = await db.UsersMembers.ToListAsync();

        //  Clientes
        if (!await db.Clientes.AnyAsync())
        {
            var clienteUser = users.FirstOrDefault(u => u.Username == "cliente1");
            if (clienteUser != null)
            {
                db.Clientes.Add(new Cliente
                {
                    Id = new IdVO(clienteUser.Id),
                    Nombre = new NombreVO("Juan Pérez"),
                    Correo = new CorreoVO(clienteUser.Email!),
                    Telefono = new TelefonoVO("3001234567"),
                    Direccion = new DireccionVO("Cra 12 #45-67, Bogotá"),
                    User = clienteUser,
                    IsActive = new EstadoVO(true)
                });
            }
        }

        //  Mecanicos
        if (!await db.Mecanicos.AnyAsync())
        {
            var mecUser = users.FirstOrDefault(u => u.Username == "mecanico1");
            if (mecUser != null)
            {
                db.Mecanicos.Add(new Mecanico
                {
                    Id = new IdVO(mecUser.Id),
                    Nombre = new NombreVO("Carlos Gómez"),
                    Telefono = new TelefonoVO("3017654321"),
                    Especialidad = new EspecialidadVO("Frenos y Suspensión"),
                    User = mecUser,
                    IsActive = new EstadoVO(true)
                });
            }
        }

        // Administradores
        if (!await db.Administradores.AnyAsync())
        {
            var adminUser = users.FirstOrDefault(u => u.Username == "admin1");
            if (adminUser != null)
            {
                db.Administradores.Add(new Administrador
                {
                    Id = new IdVO(adminUser.Id),
                    Nombre = new NombreVO("Laura Torres"),
                    Telefono = new TelefonoVO("3025556666"),
                    NivelAcceso = new NivelAccesoVO("Total"),
                    AreaResponsabilidad = new DescripcionVO("Gestión General"),
                    User = adminUser,
                    IsActive = new EstadoVO(true)
                });
            }
        }

        // Proveedores
        if (!await db.Proveedores.AnyAsync())
        {
            var provUser = users.FirstOrDefault(u => u.Username == "proveedor1");
            if (provUser != null)
            {
                db.Proveedores.Add(new Proveedor
                {
                    Id = new IdVO(provUser.Id),
                    Nombre = new NombreVO("Repuestos ABC"),
                    Telefono = new TelefonoVO("3041112222"),
                    Correo = new CorreoVO(provUser.Email!),
                    Direccion = new DireccionVO("Zona Industrial 45"),
                    User = provUser,
                    IsActive = new EstadoVO(true)
                });
            }
        }
    }

    // -------------------------------------------------------
    private static async Task SeedUserRolesAsync(AppDbContext db)
    {
        if (await db.UserMemberRols.AnyAsync()) return;

        var users = await db.UsersMembers.ToListAsync();
        var roles = await db.Roles.ToListAsync();

        var userRoles = new List<UserMemberRol>
        {
            new UserMemberRol { UserMemberId = users.First(u => u.Username == "admin1").Id, RolId = roles.First(r => r.Name == "Administrador").Id },
            new UserMemberRol { UserMemberId = users.First(u => u.Username == "cliente1").Id, RolId = roles.First(r => r.Name == "Cliente").Id },
            new UserMemberRol { UserMemberId = users.First(u => u.Username == "mecanico1").Id, RolId = roles.First(r => r.Name == "Mecanico").Id },
            new UserMemberRol { UserMemberId = users.First(u => u.Username == "proveedor1").Id, RolId = roles.First(r => r.Name == "Proveedor").Id }
        };

        db.UserMemberRols.AddRange(userRoles);
        await db.SaveChangesAsync();
    }

    // -------------------------------------------------------
    private static async Task SeedCatalogsAsync(AppDbContext db)
    {
        // Estados Cita
        if (!await db.EstadosCita.AnyAsync())
            db.EstadosCita.AddRange(new[]
            {
                new EstadoCita { Nombre = new NombreVO("Pendiente") },
                new EstadoCita { Nombre = new NombreVO("Confirmada") },
                new EstadoCita { Nombre = new NombreVO("Finalizada") },
                new EstadoCita { Nombre = new NombreVO("Cancelada") }
            });

        // Estados Orden
        if (!await db.EstadosOrden.AnyAsync())
            db.EstadosOrden.AddRange(new[]
            {
                new EstadoOrden { Nombre = new NombreVO("Pendiente") },
                new EstadoOrden { Nombre = new NombreVO("En Proceso") },
                new EstadoOrden { Nombre = new NombreVO("Completada") },
                new EstadoOrden { Nombre = new NombreVO("Cancelada") }
            });

        // Estados Pago
        if (!await db.EstadosPago.AnyAsync())
            db.EstadosPago.AddRange(new[]
            {
                new EstadoPago { Nombre = new NombreVO("Pendiente") },
                new EstadoPago { Nombre = new NombreVO("Pagado") },
                new EstadoPago { Nombre = new NombreVO("Rechazado") }
            });

        // Tipos Movimiento
        if (!await db.TiposMovimiento.AnyAsync())
            db.TiposMovimiento.AddRange(new[]
            {
                new TipoMovimiento { Nombre = new NombreVO("Ingreso") },
                new TipoMovimiento { Nombre = new NombreVO("Salida") }
            });

        // Métodos Pago
        if (!await db.MetodosPago.AnyAsync())
            db.MetodosPago.AddRange(new[]
            {
                new MetodoPago { Nombre = new NombreVO("Efectivo") },
                new MetodoPago { Nombre = new NombreVO("Tarjeta") },
                new MetodoPago { Nombre = new NombreVO("Transferencia") }
            });

        // Tipos Servicio
        if (!await db.TiposServicio.AnyAsync())
            db.TiposServicio.AddRange(new[]
            {
                new TipoServicio
                {
                    Nombre = new NombreVO("Cambio de Aceite"),
                    Descripcion = new DescripcionVO("Cambio de aceite y filtro"),
                    PrecioBase = new DineroVO(120000)
                },
                new TipoServicio
                {
                    Nombre = new NombreVO("Alineación"),
                    Descripcion = new DescripcionVO("Alineación de ruedas"),
                    PrecioBase = new DineroVO(80000)
                },
                new TipoServicio
                {
                    Nombre = new NombreVO("Balanceo"),
                    Descripcion = new DescripcionVO("Balanceo de ruedas"),
                    PrecioBase = new DineroVO(60000)
                },
                new TipoServicio
                {
                    Nombre = new NombreVO("Diagnóstico"),
                    Descripcion = new DescripcionVO("Revisión general del vehículo"),
                    PrecioBase = new DineroVO(100000)
                }
            });

        await db.SaveChangesAsync();
    }

    // -------------------------------------------------------
    private static async Task SeedVehiculosAsync(AppDbContext db)
    {
        if (await db.Vehiculos.AnyAsync()) return;

        var cliente = await db.Clientes.FirstOrDefaultAsync();
        if (cliente == null) return;

        db.Vehiculos.Add(new Vehiculo
        {
            ClienteId = cliente.Id,
            Marca = new NombreVO("Toyota"),
            Modelo = new NombreVO("Corolla"),
            Anio = new AnioVehiculoVO(2020),
            Vin = new VinVO("JTDBR32E820123456"),
            Kilometraje = new KilometrajeVO(45000)
        });

        await db.SaveChangesAsync();
    }

    // -------------------------------------------------------
    private static async Task SeedRepuestosAsync(AppDbContext db)
    {
        // Evitar duplicados
        if (await db.Repuestos.AnyAsync()) return;

        // Buscar proveedor existente
        var proveedor = await db.Proveedores.FirstOrDefaultAsync();
        if (proveedor == null) return;

        // Crear repuestos con VO en lugar de strings
        db.Repuestos.AddRange(new[]
        {
            new Repuesto
            {
                Codigo = new CodigoRepuestoVO("REP001"),
                Descripcion = new DescripcionVO("Filtro de aceite"),
                CantidadStock = new CantidadVO(50),
                PrecioUnitario = new DineroVO(20000),
                ProveedorId = proveedor.Id
            },
            new Repuesto
            {
                Codigo = new CodigoRepuestoVO("REP002"),
                Descripcion = new DescripcionVO("Pastillas de freno"),
                CantidadStock = new CantidadVO(30),
                PrecioUnitario = new DineroVO(45000),
                ProveedorId = proveedor.Id
            },
            new Repuesto
            {
                Codigo = new CodigoRepuestoVO("REP003"),
                Descripcion = new DescripcionVO("Batería 12V"),
                CantidadStock = new CantidadVO(10),
                PrecioUnitario = new DineroVO(150000),
                ProveedorId = proveedor.Id
            }
        });

        await db.SaveChangesAsync();
    }

    // -------------------------------------------------------
    private static async Task SeedOrdenesYFacturasAsync(AppDbContext db)
    {
        if (await db.OrdenesServicio.AnyAsync()) return;

        var vehiculo = await db.Vehiculos.FirstOrDefaultAsync();
        var mecanico = await db.Mecanicos.FirstOrDefaultAsync();
        var tipoServicio = await db.TiposServicio.FirstOrDefaultAsync();
        var estadoOrden = await db.EstadosOrden.FirstOrDefaultAsync();

        if (vehiculo == null || mecanico == null || tipoServicio == null || estadoOrden == null) return;

        // Crear la orden usando VO para fechas
        var orden = new OrdenServicio
        {
            VehiculoId = vehiculo.Id,
            MecanicoId = mecanico.Id,
            TipoServicioId = tipoServicio.Id,
            EstadoId = estadoOrden.Id,
            FechaIngreso = new FechaHistoricaVO(DateTime.UtcNow),
            FechaEntregaEstimada = new FechaHistoricaVO(DateTime.UtcNow.AddDays(2))
        };

        db.OrdenesServicio.Add(orden);
        await db.SaveChangesAsync();

        // Detalle con Value Object para costo
        var repuesto = await db.Repuestos.FirstOrDefaultAsync();
        if (repuesto != null)
        {
            db.DetallesOrden.Add(new DetalleOrden
            {
                OrdenServicioId = orden.Id,
                RepuestoId = repuesto.Id,
                Cantidad = new CantidadVO(2),
                Costo = new DineroVO(repuesto.PrecioUnitario.Value * 2)
            });

            await db.SaveChangesAsync();
        }

        // Calcular subtotal en base al valor interno del VO
        var subtotal = await db.DetallesOrden
            .Where(d => d.OrdenServicioId == orden.Id)
            .SumAsync(d => d.Costo.Value);

        // Factura con fechas y dinero como VO
        db.Facturas.Add(new Factura
        {
            OrdenServicioId = orden.Id,
            MontoRepuestos = new DineroVO(subtotal),
            ManoObra = new DineroVO(50000),
            Total = new DineroVO(subtotal + 50000),
            FechaGeneracion = new FechaHistoricaVO(DateTime.UtcNow)
        });

        await db.SaveChangesAsync();

        // Pago con VO
        var factura = await db.Facturas.FirstOrDefaultAsync();
        var metodo = await db.MetodosPago.FirstOrDefaultAsync();
        var estadoPago = await db.EstadosPago.FirstOrDefaultAsync();

        if (factura != null && metodo != null && estadoPago != null)
        {
            db.Pagos.Add(new Pago
            {
                FacturaId = factura.Id,
                MetodoPagoId = metodo.Id,
                EstadoPagoId = estadoPago.Id,
                Monto = factura.Total,
                FechaPago = new FechaHistoricaVO(DateTime.UtcNow)
            });

            await db.SaveChangesAsync();
        }
    }

    // -------------------------------------------------------
    private static async Task SeedHistorialInventarioAsync(AppDbContext db)
    {
        if (await db.HistorialesInventario.AnyAsync()) return;

        var repuesto = await db.Repuestos.FirstOrDefaultAsync();
        var admin = await db.Administradores.FirstOrDefaultAsync();
        var tipoMov = await db.TiposMovimiento.FirstOrDefaultAsync();

        if (repuesto == null || admin == null || tipoMov == null) return;

        db.HistorialesInventario.Add(new HistorialInventario
        {
            RepuestoId = repuesto.Id,
            AdminId = admin.Id,
            TipoMovimientoId = tipoMov.Id,
            Cantidad = new CantidadVO(10),
            Observaciones = new DescripcionVO("Carga inicial de stock")
        });

        await db.SaveChangesAsync();
    }
}
