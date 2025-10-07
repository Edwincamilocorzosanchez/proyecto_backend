using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;
public class CitaRepository : ICitaRepository
{
    private readonly AppDbContext _context;

    public CitaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cita?> GetByIdAsync(IdVO id, CancellationToken ct = default)
    {
        return await _context.Citas
            .Include(c => c.Cliente)
            .Include(c => c.Vehiculo)
            .FirstOrDefaultAsync(c => c.Id.Value == id.Value, ct);
    }

    public async Task<IReadOnlyList<Cita>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Citas
            .Include(c => c.Cliente)
            .Include(c => c.Vehiculo)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Cita>> GetByClienteIdAsync(IdVO clienteId, CancellationToken ct = default)
    {
        return await _context.Citas
            .Include(c => c.Cliente)
            .Include(c => c.Vehiculo)
            .Where(c => c.ClienteId == clienteId)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Cita>> GetByVehiculoIdAsync(IdVO vehiculoId, CancellationToken ct = default)
    {
        return await _context.Citas
            .Include(c => c.Cliente)
            .Include(c => c.Vehiculo)
            .Where(c => c.VehiculoId == vehiculoId)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Cita>> GetByFechaAsync(FechaCitaVO fecha, CancellationToken ct = default)
    {
        return await _context.Citas
            .Include(c => c.Cliente)
            .Include(c => c.Vehiculo)
            .Where(c => c.FechaCita == fecha)
            .ToListAsync(ct);
    }

    public async Task<int> AddAsync(Cita cita, CancellationToken ct = default)
    {
        await _context.Citas.AddAsync(cita, ct);
        await _context.SaveChangesAsync(ct);
        return cita.Id.Value.GetHashCode();
    }

    public async Task<bool> UpdateAsync(Cita cita, CancellationToken ct = default)
    {
        _context.Citas.Update(cita);
        var updated = await _context.SaveChangesAsync(ct);
        return updated > 0;
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
    {
        var cita = await _context.Citas.FirstOrDefaultAsync(c => c.Id.Value == id.Value, ct);
        if (cita == null) return false;

        _context.Citas.Remove(cita);
        var deleted = await _context.SaveChangesAsync(ct);
        return deleted > 0;
    }
}