using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Interfaces;

public interface IHistorialInventarioService
{
    Task<HistorialInventario?> GetByIdAsync(IdVO id, CancellationToken ct = default);
    Task<IReadOnlyList<HistorialInventario>> GetAllAsync(CancellationToken ct = default);

    Task<int> AddAsync(HistorialInventario historial, CancellationToken ct = default);
    
    // Métodos adicionales de negocio
    Task<IReadOnlyList<HistorialInventario>> GetByRepuestoIdAsync(IdVO repuestoId, CancellationToken ct = default);
    Task<IReadOnlyList<HistorialInventario>> GetByAdminIdAsync(IdVO adminId, CancellationToken ct = default);
    Task<IReadOnlyList<HistorialInventario>> GetByTipoMovimientoAsync(IdVO tipoMovimientoId, CancellationToken ct = default);
}
