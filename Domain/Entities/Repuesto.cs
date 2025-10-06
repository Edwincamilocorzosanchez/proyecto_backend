using Domain.ValueObjects;

namespace Domain.Entities;

public class Repuesto : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public CodigoRepuestoVO Codigo { get; set; } = null!;
    public DescripcionVO Descripcion { get; set; } = null!;
    public CantidadVO CantidadStock { get; set; } = null!;
    public DineroVO PrecioUnitario { get; set; } = null!;
    public IdVO? ProveedorId { get; set; }

    // Relaciones
    public Proveedor? Proveedor { get; set; }
    public ICollection<HistorialInventario> Historiales { get; set; } = new List<HistorialInventario>();
    public ICollection<DetalleOrden> DetallesOrden { get; set; } = new List<DetalleOrden>();
}
