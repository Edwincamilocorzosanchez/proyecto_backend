namespace Domain.Entities;

public class HistorialInventario
{
    public int Id { get; set; }
    public int RepuestoId { get; set; }
    public int? AdminId { get; set; }
    public int TipoMovimientoId { get; set; }
    public int Cantidad { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public string? Observaciones { get; set; }

    // Relaciones
    public Repuesto Repuesto { get; set; } = null!;
    public Administrador? Administrador { get; set; }
    public TipoMovimiento TipoMovimiento { get; set; } = null!;
}
