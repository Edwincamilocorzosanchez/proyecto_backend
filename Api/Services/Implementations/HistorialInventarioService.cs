using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class HistorialInventarioService : IHistorialInventarioService
{
    private readonly IHistorialInventarioRepository _repository;

    public HistorialInventarioService(IHistorialInventarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<HistorialInventario?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<HistorialInventario>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<int> AddAsync(HistorialInventario historial, CancellationToken ct = default)
        => await _repository.AddAsync(historial, ct);

    public async Task<IReadOnlyList<HistorialInventario>> GetByRepuestoIdAsync(IdVO repuestoId, CancellationToken ct = default)
        => await _repository.GetByRepuestoIdAsync(repuestoId, ct);

    public async Task<IReadOnlyList<HistorialInventario>> GetByAdminIdAsync(IdVO adminId, CancellationToken ct = default)
        => await _repository.GetByAdminIdAsync(adminId, ct);

    public async Task<IReadOnlyList<HistorialInventario>> GetByTipoMovimientoAsync(IdVO tipoMovimientoId, CancellationToken ct = default)
        => await _repository.GetByTipoMovimientoAsync(tipoMovimientoId, ct);
}
