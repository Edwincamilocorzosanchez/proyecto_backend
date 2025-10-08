using Api.Services.Interfaces;
using Application.Abstractions;
using Domain.Entities;
using Domain.ValueObjects;

namespace Api.Services.Implementations;

public class FacturaService : IFacturaService
{
    private readonly IFacturaRepository _repository;
    private readonly IDetalleOrdenRepository _detalleRepository;

    public FacturaService(IFacturaRepository repository, IDetalleOrdenRepository detalleRepository)
    {
        _repository = repository;
        _detalleRepository = detalleRepository;
    }

    public async Task<Factura?> GetByIdAsync(IdVO id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task<IReadOnlyList<Factura>> GetAllAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<int> AddAsync(Factura factura, CancellationToken ct = default)
        => await _repository.AddAsync(factura, ct);

    public async Task<bool> UpdateAsync(Factura factura, CancellationToken ct = default)
        => await _repository.UpdateAsync(factura, ct);

    public async Task<bool> DeleteAsync(IdVO id, CancellationToken ct = default)
        => await _repository.DeleteAsync(id, ct);

    public async Task<decimal> CalcularTotalAsync(IdVO facturaId, CancellationToken ct = default)
    {
        var factura = await _repository.GetByIdAsync(facturaId, ct);
        if (factura == null) return 0;

        var detalles = await _detalleRepository.GetByOrdenIdAsync(factura.OrdenServicioId, ct);
        decimal totalRepuestos = detalles.Sum(d => d.Costo.Value * d.Cantidad.Value);

        return totalRepuestos + factura.ManoObra.Value;
    }

    public async Task<IReadOnlyList<Factura>> GetByOrdenServicioIdAsync(IdVO ordenServicioId, CancellationToken ct = default)
        => await _repository.GetByOrdenServicioIdAsync(ordenServicioId, ct);
}
