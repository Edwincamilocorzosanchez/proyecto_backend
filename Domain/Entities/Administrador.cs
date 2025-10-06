using Domain.Entities.Auth;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Administrador : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
    public TelefonoVO? Telefono { get; set; }
    public NivelAccesoVO? NivelAcceso { get; set; }
    public DescripcionVO? AreaResponsabilidad { get; set; }
    public EstadoVO IsActive { get; set; } = null!;

    // relaciones
    public UserMember User { get; set; } = null!;
    // constructores 
    public Administrador() { }
    public Administrador(IdVO id, NombreVO nombre, TelefonoVO? telefono, NivelAccesoVO? nivelAcceso, DescripcionVO? areaResponsabilidad, EstadoVO isActive)
    {
        Id = id;
        Nombre = nombre;
        Telefono = telefono;
        NivelAcceso = nivelAcceso;
        AreaResponsabilidad = areaResponsabilidad;
        IsActive = isActive;
    }
}
