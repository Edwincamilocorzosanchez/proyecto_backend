namespace Domain.Entities;

public class Repuesto : BaseEntity
{
    public int Id { get; set; }
    public string Codigo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public int CantidadStock { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int? ProveedorId { get; set; }

    // Relaciones
    public Proveedor? Proveedor { get; set; }
    public ICollection<HistorialInventario> Historiales { get; set; } = new List<HistorialInventario>();
    public ICollection<DetalleOrden> DetallesOrden { get; set; } = new List<DetalleOrden>();
}
