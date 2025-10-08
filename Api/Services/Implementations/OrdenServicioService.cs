using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class OrdenServicioService : IOrdenServicioService
{
    private readonly IOrdenServicioRepository _repository;

    public OrdenServicioService(IOrdenServicioRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrdenServicio?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<OrdenServicio>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<int> AddAsync(OrdenServicio ordenServicio, CancellationToken ct = default)
    {
        // Validaciones básicas
        if (ordenServicio.FechaEntregaEstimada.Value < ordenServicio.FechaIngreso.Value)
            throw new Exception("La fecha de entrega estimada no puede ser menor a la fecha de ingreso.");

        return await _repository.AddAsync(ordenServicio, ct);
    }

    public async Task<bool> UpdateAsync(OrdenServicio ordenServicio, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(ordenServicio.Id, ct);
        if (existing == null)
            return false;

        if (ordenServicio.FechaEntregaEstimada.Value < ordenServicio.FechaIngreso.Value)
            throw new Exception("La fecha de entrega estimada no puede ser menor a la fecha de ingreso.");

        return await _repository.UpdateAsync(ordenServicio, ct);
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing == null)
            return false;

        return await _repository.DeleteAsync(id, ct);
    }

    public async Task<IReadOnlyList<OrdenServicio>> GetByVehiculoAsync(IdVO vehiculoId, CancellationToken ct = default)
        => await _repository.GetByVehiculoAsync(vehiculoId, ct);

    public async Task<IReadOnlyList<OrdenServicio>> GetByMecanicoAsync(IdVO mecanicoId, CancellationToken ct = default)
        => await _repository.GetByMecanicoAsync(mecanicoId, ct);
}
