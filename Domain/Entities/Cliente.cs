using Domain.Entities.Auth;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Cliente : BaseEntity
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
    public CorreoVO Correo { get; set; } = null!;
    public TelefonoVO Telefono { get; set; } = null!;
    public DireccionVO Direccion { get; set; } = null!;
    // valor por defecto true
    public EstadoVO IsActive { get; set; } = new EstadoVO(true);

    // Clave foránea real (int)
    public int UserId { get; set; }
    // Relación 1:1 con usuario
    public UserMember User { get; set; } = null!;
    // relacion uno a muchos
    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();

    // constructores
    public Cliente()
    {
        IsActive = new EstadoVO(true); // garantiza que nunca será null
    }
    public Cliente(IdVO id, NombreVO nombre, CorreoVO correo, TelefonoVO telefono, DireccionVO direccion, EstadoVO? isActive, int userId)
    {
        Id = id;
        Nombre = nombre;
        Correo = correo;
        Telefono = telefono;
        Direccion = direccion;
        IsActive = isActive ?? new EstadoVO(true); // valor por defecto si no se pasa
        UserId = userId;
    }
}
