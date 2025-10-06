namespace Domain.Entities;

public class Vehiculo : BaseEntity
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Marca { get; set; } = null!;
    public string Modelo { get; set; } = null!;
    public short Anio { get; set; }
    public string Vin { get; set; } = null!;
    public int Kilometraje { get; set; }

    // Relaciones
    public Cliente Cliente { get; set; } = null!;
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();
}
