using Domain.ValueObjects;

namespace Domain.Entities;

public class DetalleOrden
{
    public IdVO OrdenServicioId { get; set; } = null!;
    public IdVO RepuestoId { get; set; } = null!;
    public CantidadVO Cantidad { get; set; } = null!;
    public DineroVO Costo { get; set; } = null!;

    // Relaciones
    public OrdenServicio OrdenServicio { get; set; } = null!;
    public Repuesto Repuesto { get; set; } = null!;
}
