using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class CitaService : ICitaService
{
    private readonly ICitaRepository _repository;

    public CitaService(ICitaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Cita?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<Cita>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<IReadOnlyList<Cita>> GetByClienteIdAsync(IdVO clienteId, CancellationToken ct = default)
        => await _repository.GetByClienteIdAsync(clienteId, ct);

    public async Task<IReadOnlyList<Cita>> GetByVehiculoIdAsync(IdVO vehiculoId, CancellationToken ct = default)
        => await _repository.GetByVehiculoIdAsync(vehiculoId, ct);

    public async Task<int> AddAsync(Cita cita, CancellationToken ct = default)
    {
        // Validación: la fecha de la cita debe ser futura
        if (cita.FechaCita.Value <= DateTime.UtcNow)
            throw new Exception("La fecha de la cita debe ser futura.");

        return await _repository.AddAsync(cita, ct);
    }

    public async Task<bool> UpdateAsync(Cita cita, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(cita.Id, ct);
        if (existing == null)
            return false;

        // Validación: no permitir actualizar a una fecha pasada
        if (cita.FechaCita.Value <= DateTime.UtcNow)
            throw new Exception("La fecha de la cita debe ser futura.");

        return await _repository.UpdateAsync(cita, ct);
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing == null)
            return false;

        return await _repository.DeleteAsync(id, ct);
    }
}
