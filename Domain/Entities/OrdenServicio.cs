namespace Domain.Entities;

public class OrdenServicio : BaseEntity
{
    public int Id { get; set; }
    public int VehiculoId { get; set; }
    public int MecanicoId { get; set; }
    public int TipoServicioId { get; set; }
    public int EstadoId { get; set; }
    public DateTime FechaIngreso { get; set; }
    public DateTime FechaEntregaEstimada { get; set; }

    // Relaciones
    public Vehiculo Vehiculo { get; set; } = null!;
    public Mecanico Mecanico { get; set; } = null!;
    public TipoServicio TipoServicio { get; set; } = null!;
    public EstadoOrden Estado { get; set; } = null!;

    public ICollection<DetalleOrden> Detalles { get; set; } = new List<DetalleOrden>();
    public ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
