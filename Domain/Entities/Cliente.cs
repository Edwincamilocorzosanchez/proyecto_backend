using Domain.Entities.Auth;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Cliente : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public TelefonoVO? Telefono { get; set; }
    public DireccionVO? Direccion { get; set; }
    public EstadoVO IsActive { get; set; } = null!;

    // Relación 1:1 con usuario
    public UserMember User { get; set; } = null!;
}
