using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class ProveedorService : IProveedorService
{
    private readonly IProveedorRepository _repository;

    public ProveedorService(IProveedorRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddAsync(Proveedor proveedor, CancellationToken ct = default)
    {
        if (await _repository.ExistsByNombreAsync(proveedor.Nombre, ct))
            throw new Exception($"Ya existe un proveedor con el nombre '{proveedor.Nombre.Value}'");

        return await _repository.AddAsync(proveedor, ct);
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
        => await _repository.DeleteAsync(id, ct);

    public async Task<IReadOnlyList<Proveedor>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<IReadOnlyList<Proveedor>> GetActivosAsync(CancellationToken ct = default)
        => await _repository.GetActivosAsync(ct);

    public async Task<Proveedor?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<bool> UpdateAsync(Proveedor proveedor, CancellationToken ct = default)
    {
        if (await _repository.ExistsByNombreAsync(proveedor.Nombre, ct))
            throw new Exception($"Ya existe un proveedor con el nombre '{proveedor.Nombre.Value}'");

        return await _repository.UpdateAsync(proveedor, ct);
    }
    public async Task<bool> ExistsByNombreAsync(NombreVO nombre, CancellationToken ct = default)
        => await _repository.ExistsByNombreAsync(nombre, ct);
}
