using Domain.Entities.Auth;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Proveedor : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
    public TelefonoVO? Telefono { get; set; }
    public CorreoVO? Correo { get; set; }
    public DireccionVO? Direccion { get; set; }
    public EstadoVO IsActive { get; set; } = null!;

    // relaciones
    public UserMember User { get; set; } = null!;
}
