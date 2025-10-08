using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class PagoService : IPagoService
{
    private readonly IPagoRepository _repository;
    private readonly IFacturaRepository _facturaRepository;

    public PagoService(IPagoRepository repository, IFacturaRepository facturaRepository)
    {
        _repository = repository;
        _facturaRepository = facturaRepository;
    }

    public async Task<Pago?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<Pago>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<int> AddAsync(Pago pago, CancellationToken ct = default)
    {
        // alidaciones
        if (pago.Monto.Value <= 0)
            throw new ArgumentException("El monto del pago debe ser mayor a cero.");

        var factura = await _facturaRepository.GetByIdAsync(pago.FacturaId, ct);
        if (factura == null)
            throw new InvalidOperationException("No se puede registrar el pago porque la factura no existe.");

        var totalPagado = await GetTotalPagadoPorFacturaAsync(pago.FacturaId, ct);
        if (totalPagado + pago.Monto.Value > factura.Total.Value)
            throw new InvalidOperationException("El pago excede el total pendiente de la factura.");

        if (pago.FechaPago == null)
            pago.FechaPago = new FechaHistoricaVO(DateTime.UtcNow);

        return await _repository.AddAsync(pago, ct);
    }

    public async Task<bool> UpdateAsync(Pago pago, CancellationToken ct = default)
    {
        if (pago.Monto.Value <= 0)
            throw new ArgumentException("El monto del pago debe ser mayor a cero.");

        var factura = await _facturaRepository.GetByIdAsync(pago.FacturaId, ct);
        if (factura == null)
            throw new InvalidOperationException("La factura asociada no existe.");

        var totalPagadoSinEste = await GetTotalPagadoPorFacturaAsync(pago.FacturaId, ct) - pago.Monto.Value;
        if (totalPagadoSinEste + pago.Monto.Value > factura.Total.Value)
            throw new InvalidOperationException("El pago actualizado excede el total pendiente de la factura.");

        return await _repository.UpdateAsync(pago, ct);
    }

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
    {
        var pago = await _repository.GetByIdAsync(id, ct);
        if (pago == null)
            throw new InvalidOperationException("No se puede eliminar un pago que no existe.");

        return await _repository.DeleteAsync(id, ct);
    }

    public async Task<decimal> GetTotalPagadoPorFacturaAsync(IdVO facturaId, CancellationToken ct = default)
    {
        var pagos = await _repository.GetByFacturaIdAsync(facturaId, ct);
        return pagos.Sum(p => p.Monto.Value);
    }

    public async Task<IReadOnlyList<Pago>> GetByFacturaIdAsync(IdVO facturaId, CancellationToken ct = default)
        => await _repository.GetByFacturaIdAsync(facturaId, ct);
}
