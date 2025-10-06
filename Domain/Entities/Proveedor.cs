namespace Domain.Entities;

public class Proveedor : BaseEntity
{
    public int Id { get; set; } // FK hacia UsersMembers
    public string Nombre { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
    public bool IsActive { get; set; } = true;

    public UserMember User { get; set; } = null!;
}
