using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class VehiculoService : IVehiculoService
{
    private readonly IVehiculoRepository _repository;

    public VehiculoService(IVehiculoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Vehiculo?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<Vehiculo>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<IReadOnlyList<Vehiculo>> GetByClienteIdAsync(IdVO clienteId, CancellationToken ct = default)
        => await _repository.GetByClienteIdAsync(clienteId, ct);

    public async Task<int> AddAsync(Vehiculo vehiculo, CancellationToken ct = default)
    {
        // Validar que el VIN no exista
        if (await _repository.ExistsByVinAsync(vehiculo.Vin, ct))
            throw new Exception($"Ya existe un vehículo con VIN '{vehiculo.Vin.Value}'");

        return await _repository.AddAsync(vehiculo, ct);
    }

    public async Task<bool> UpdateAsync(Vehiculo vehiculo, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(vehiculo.Id, ct);
        if (existing == null)
            return false;

        // Validar VIN único
        if (existing.Vin.Value != vehiculo.Vin.Value &&
            await _repository.ExistsByVinAsync(vehiculo.Vin, ct))
        {
            throw new Exception($"Ya existe un vehículo con VIN '{vehiculo.Vin.Value}'");
        }

        return await _repository.UpdateAsync(vehiculo, ct);
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing == null)
            return false;

        return await _repository.DeleteAsync(id, ct);
    }

    public async Task<bool> ExistsByVinAsync(VinVO vin, CancellationToken ct = default)
        => await _repository.ExistsByVinAsync(vin, ct);
}
