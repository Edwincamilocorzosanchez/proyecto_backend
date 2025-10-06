using Domain.Entities.Auth;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Cliente : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public TelefonoVO Telefono { get; set; } = null!;
    public DireccionVO Direccion { get; set; } = null!;
    public EstadoVO IsActive { get; set; } = null!;

    // Relación 1:1 con usuario
    public UserMember User { get; set; } = null!;
    // relacion uno a muchos
    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();

    // constructores
    public Cliente() { }
    public Cliente(IdVO id, TelefonoVO telefono, DireccionVO direccion, EstadoVO isActive)
    {
        Id = id;
        Telefono = telefono;
        Direccion = direccion;
        IsActive = isActive;
    }
}
