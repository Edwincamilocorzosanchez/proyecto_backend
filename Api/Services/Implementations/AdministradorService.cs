using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class AdministradorService : IAdministradorService
{
    private readonly IAdministradorRepository _repository;

    public AdministradorService(IAdministradorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Administrador?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<Administrador>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<IReadOnlyList<Administrador>> GetActiveAsync(CancellationToken ct = default)
    {
        var all = await _repository.GetAllAsync(ct);
        return all.Where(a => a.IsActive.Value).ToList();
    }

    public async Task<int> AddAsync(Administrador administrador, CancellationToken ct = default)
    {
        if (await _repository.ExistsByNombreAsync(administrador.Nombre, ct))
            throw new Exception($"Ya existe un administrador con el nombre '{administrador.Nombre.Value}'");

        return await _repository.AddAsync(administrador, ct);
    }

    public async Task<bool> UpdateAsync(Administrador administrador, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(administrador.Id, ct);
        if (existing == null)
            return false;

        // Validar que no duplique nombres
        if (existing.Nombre.Value != administrador.Nombre.Value &&
            await _repository.ExistsByNombreAsync(administrador.Nombre, ct))
        {
            throw new Exception($"Ya existe un administrador con el nombre '{administrador.Nombre.Value}'");
        }

        return await _repository.UpdateAsync(administrador, ct);
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing == null)
            return false;

        return await _repository.DeleteAsync(id, ct);
    }

    public async Task<bool> ExistsByNombreAsync(NombreVO nombre, CancellationToken ct = default)
        => await _repository.ExistsByNombreAsync(nombre, ct);
}
