using Domain.Entities.Auth;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Mecanico : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
    public TelefonoVO? Telefono { get; set; }
    public EspecialidadVO? Especialidad { get; set; }
    public EstadoVO IsActive { get; set; } = null!;
    // relaciones
    public UserMember User { get; set; } = null!;
    // relacion uno a muchos
    public ICollection<OrdenServicio> OrdenesServicio { get; set; } = new List<OrdenServicio>();
    // constructores
    public Mecanico() { }
    public Mecanico(IdVO id, NombreVO nombre, TelefonoVO? telefono, EspecialidadVO? especialidad, EstadoVO isActive)
    {
        Id = id;
        Nombre = nombre;
        Telefono = telefono;
        Especialidad = especialidad;
        IsActive = isActive;
    }
}
