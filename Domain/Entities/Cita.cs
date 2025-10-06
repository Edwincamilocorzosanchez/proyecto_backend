namespace Domain.Entities;

public class Cita : BaseEntity
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int VehiculoId { get; set; }
    public DateTime FechaCita { get; set; }
    public string? Motivo { get; set; }
    public int EstadoId { get; set; }

    // Relaciones
    public Cliente Cliente { get; set; } = null!;
    public Vehiculo Vehiculo { get; set; } = null!;
    public EstadoCita Estado { get; set; } = null!;
}
