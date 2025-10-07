using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class DetalleOrdenRepository : IDetalleOrdenRepository
{
    private readonly AppDbContext _context;

    public DetalleOrdenRepository(AppDbContext context) =>_context = context;

    public async Task<DetalleOrden?> GetByIdsAsync(IdVO ordenServicioId, IdVO repuestoId, CancellationToken ct = default)
    {
        return await _context.DetallesOrden
            .Include(d => d.OrdenServicio)
            .Include(d => d.Repuesto)
            .AsNoTracking()
            .FirstOrDefaultAsync(d =>
                d.OrdenServicioId.Value == ordenServicioId.Value &&
                d.RepuestoId.Value == repuestoId.Value,
                ct);
    }

    public async Task<IReadOnlyList<DetalleOrden>> GetByOrdenServicioIdAsync(IdVO ordenServicioId, CancellationToken ct = default)
    {
        return await _context.DetallesOrden
            .Include(d => d.OrdenServicio)
            .Include(d => d.Repuesto)
            .AsNoTracking()
            .Where(d => d.OrdenServicioId.Value == ordenServicioId.Value)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<DetalleOrden>> GetByRepuestoIdAsync(IdVO repuestoId, CancellationToken ct = default)
    {
        return await _context.DetallesOrden
            .Include(d => d.OrdenServicio)
            .Include(d => d.Repuesto)
            .AsNoTracking()
            .Where(d => d.RepuestoId.Value == repuestoId.Value)
            .ToListAsync(ct);
    }

    public async Task<int> AddAsync(DetalleOrden detalle, CancellationToken ct = default)
    {
        await _context.DetallesOrden.AddAsync(detalle, ct);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> UpdateAsync(DetalleOrden detalle, CancellationToken ct = default)
    {
        var existing = await _context.DetallesOrden
            .FirstOrDefaultAsync(d =>
                d.OrdenServicioId.Value == detalle.OrdenServicioId.Value &&
                d.RepuestoId.Value == detalle.RepuestoId.Value,
                ct);

        if (existing is null)
            return false;

        _context.Entry(existing).CurrentValues.SetValues(detalle);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(IdVO ordenServicioId, IdVO repuestoId, CancellationToken ct = default)
    {
        var existing = await _context.DetallesOrden
            .FirstOrDefaultAsync(d =>
                d.OrdenServicioId.Value == ordenServicioId.Value &&
                d.RepuestoId.Value == repuestoId.Value,
                ct);

        if (existing is null)
            return false;

        _context.DetallesOrden.Remove(existing);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
