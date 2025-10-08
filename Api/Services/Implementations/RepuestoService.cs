using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class RepuestoService : IRepuestoService
{
    private readonly IRepuestoRepository _repository;

    public RepuestoService(IRepuestoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Repuesto?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<Repuesto>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<int> AddAsync(Repuesto repuesto, CancellationToken ct = default)
    {
        if (await _repository.ExistsByCodigoAsync(repuesto.Codigo, ct))
            throw new Exception("Ya existe un repuesto con ese código.");

        return await _repository.AddAsync(repuesto, ct);
    }

    public async Task<bool> UpdateAsync(Repuesto repuesto, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(repuesto.Id, ct);
        if (existing == null) return false;

        return await _repository.UpdateAsync(repuesto, ct);
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing == null) return false;

        return await _repository.DeleteAsync(id, ct);
    }

    public async Task<bool> ExistsByCodigoAsync(CodigoRepuestoVO codigo, CancellationToken ct = default)
        => await _repository.ExistsByCodigoAsync(codigo, ct);

    public async Task<bool> UpdateStockAsync(IdVO id, int cantidad, CancellationToken ct = default)
    {
        var repuesto = await _repository.GetByIdAsync(id, ct);
        if (repuesto == null) return false;

        var newCantidad = repuesto.CantidadStock.Value + cantidad;
        if (newCantidad < 0) throw new Exception("Stock insuficiente.");

        repuesto.CantidadStock = new CantidadVO(newCantidad);
        return await _repository.UpdateAsync(repuesto, ct);
    }
}
