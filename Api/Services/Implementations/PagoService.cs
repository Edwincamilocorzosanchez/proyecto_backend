using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class PagoService : IPagoService
{
    private readonly IPagoRepository _repository;

    public PagoService(IPagoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Pago?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<Pago>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<int> AddAsync(Pago pago, CancellationToken ct = default)
        => await _repository.AddAsync(pago, ct);

    public async Task<bool> UpdateAsync(Pago pago, CancellationToken ct = default)
        => await _repository.UpdateAsync(pago, ct);

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
        => await _repository.DeleteAsync(id, ct);

    public async Task<decimal> GetTotalPagadoPorFacturaAsync(IdVO facturaId, CancellationToken ct = default)
    {
        var pagos = await _repository.GetByFacturaIdAsync(facturaId, ct);
        return pagos.Sum(p => p.Monto.Value);
    }

    public async Task<IReadOnlyList<Pago>> GetByFacturaIdAsync(IdVO facturaId, CancellationToken ct = default)
        => await _repository.GetByFacturaIdAsync(facturaId, ct);
}
