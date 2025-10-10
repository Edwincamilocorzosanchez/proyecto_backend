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
    // valor por defecto true
    public EstadoVO IsActive { get; set; } = new EstadoVO(true);

    // relaciones
    // Clave foránea real (int)
    public int UserId { get; set; }
    public UserMember User { get; set; } = null!;
    // relacion uno a muchos
    public ICollection<Repuesto> Repuestos { get; set; } = new List<Repuesto>();    

    // constructores
    public Proveedor()
    {
        IsActive = new EstadoVO(true); // garantiza que nunca será null
    }
    public Proveedor(IdVO id, NombreVO nombre, TelefonoVO? telefono, CorreoVO? correo, DireccionVO? direccion, EstadoVO? isActive, int userId)
    {
        Id = id;
        Nombre = nombre;
        Telefono = telefono;
        Correo = correo;
        Direccion = direccion;
        IsActive = isActive ?? new EstadoVO(true); // valor por defecto si no se pasa
        UserId = userId;
    }
}
