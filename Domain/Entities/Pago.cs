namespace Domain.Entities;

public class Pago : BaseEntity
{
    public int Id { get; set; }
    public int FacturaId { get; set; }
    public int MetodoPagoId { get; set; }
    public int EstadoPagoId { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }

    // Relaciones
    public Factura Factura { get; set; } = null!;
    public MetodoPago MetodoPago { get; set; } = null!;
    public EstadoPago EstadoPago { get; set; } = null!;
}
