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

    public UserMember User { get; set; } = null!;
}
