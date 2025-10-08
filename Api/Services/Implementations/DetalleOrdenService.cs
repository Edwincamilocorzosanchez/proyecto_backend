using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class DetalleOrdenService : IDetalleOrdenService
{
    private readonly IDetalleOrdenRepository _repository;

    public DetalleOrdenService(IDetalleOrdenRepository repository)
    {
        _repository = repository;
    }

    public async Task<DetalleOrden?> GetByIdAsync(IdVO ordenServicioId, IdVO repuestoId, CancellationToken ct = default)
        => await _repository.GetByIdsAsync(ordenServicioId, repuestoId, ct);

    public async Task<IReadOnlyList<DetalleOrden>> GetByOrdenIdAsync(IdVO ordenServicioId, CancellationToken ct = default)
        => await _repository.GetByOrdenServicioIdAsync(ordenServicioId, ct);

    public async Task<int> AddAsync(DetalleOrden detalle, CancellationToken ct = default)
        => await _repository.AddAsync(detalle, ct);

    public async Task<bool> UpdateAsync(DetalleOrden detalle, CancellationToken ct = default)
        => await _repository.UpdateAsync(detalle, ct);

    public async Task<bool> DeleteAsync(IdVO ordenServicioId, IdVO repuestoId, CancellationToken ct = default)
        => await _repository.DeleteAsync(ordenServicioId, repuestoId, ct);
}
