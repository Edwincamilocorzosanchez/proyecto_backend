using Domain.ValueObjects;

namespace Domain.Entities;

public class Pago : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public IdVO FacturaId { get; set; } = null!;
    public IdVO MetodoPagoId { get; set; } = null!;
    public IdVO EstadoPagoId { get; set; } = null!;
    public DineroVO Monto { get; set; } = null!;
    public FechaHistoricaVO FechaPago { get; set; } = null!;

    // Relaciones
    public Factura Factura { get; set; } = null!;
    public MetodoPago MetodoPago { get; set; } = null!;
    public EstadoPago EstadoPago { get; set; } = null!;
}
