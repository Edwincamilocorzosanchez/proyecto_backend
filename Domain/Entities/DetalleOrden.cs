namespace Domain.Entities;

public class DetalleOrden
{
    public int OrdenServicioId { get; set; }
    public int RepuestoId { get; set; }
    public int Cantidad { get; set; }
    public decimal Costo { get; set; }

    // Relaciones
    public OrdenServicio OrdenServicio { get; set; } = null!;
    public Repuesto Repuesto { get; set; } = null!;
}
