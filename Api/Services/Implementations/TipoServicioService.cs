using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class TipoServicioService : ITipoServicioService
{
    private readonly ITipoServicioRepository _repository;

    public TipoServicioService(ITipoServicioRepository repository)
    {
        _repository = repository;
    }

    public async Task<TipoServicio?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<TipoServicio>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<int> AddAsync(TipoServicio tipoServicio, CancellationToken ct = default)
    {
        // Validación: el precio base no puede ser negativo
        if (tipoServicio.PrecioBase.Value < 0)
            throw new Exception("El precio base no puede ser negativo.");

        return await _repository.AddAsync(tipoServicio, ct);
    }

    public async Task<bool> UpdateAsync(TipoServicio tipoServicio, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(tipoServicio.Id, ct);
        if (existing == null)
            return false;

        if (tipoServicio.PrecioBase.Value < 0)
            throw new Exception("El precio base no puede ser negativo.");

        return await _repository.UpdateAsync(tipoServicio, ct);
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing == null)
            return false;

        return await _repository.DeleteAsync(id, ct);
    }
}
