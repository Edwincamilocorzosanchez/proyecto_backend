namespace Domain.Entities;

public class Factura : BaseEntity
{
    public int Id { get; set; }
    public int OrdenServicioId { get; set; }
    public decimal MontoRepuestos { get; set; }
    public decimal ManoObra { get; set; }
    public decimal Total { get; set; }
    public DateTime FechaGeneracion { get; set; }

    // Relaciones
    public OrdenServicio OrdenServicio { get; set; } = null!;
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
