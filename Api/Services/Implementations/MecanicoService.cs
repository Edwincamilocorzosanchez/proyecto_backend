using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class MecanicoService : IMecanicoService
{
    private readonly IMecanicoRepository _repository;

    public MecanicoService(IMecanicoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Mecanico?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<Mecanico>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<IReadOnlyList<Mecanico>> GetActiveAsync(CancellationToken ct = default)
    {
        var all = await _repository.GetAllAsync(ct);
        return all.Where(m => m.IsActive.Value).ToList();
    }

    public async Task<int> AddAsync(Mecanico mecanico, CancellationToken ct = default)
    {
        if (await _repository.ExistsByNombreAsync(mecanico.Nombre, ct))
            throw new Exception($"Ya existe un mecánico con el nombre '{mecanico.Nombre.Value}'");

        return await _repository.AddAsync(mecanico, ct);
    }

    public async Task<bool> UpdateAsync(Mecanico mecanico, CancellationToken ct = default)
    {
        var existing = await _repository.GetByIdAsync(mecanico.Id, ct);
        if (existing == null)
            return false;

        // Aquí podrías agregar validaciones adicionales, por ejemplo no duplicar nombres
        if (existing.Nombre.Value != mecanico.Nombre.Value && 
            await _repository.ExistsByNombreAsync(mecanico.Nombre, ct))
        {
            throw new Exception($"Ya existe un mecánico con el nombre '{mecanico.Nombre.Value}'");
        }

        return await _repository.UpdateAsync(mecanico, ct);
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
