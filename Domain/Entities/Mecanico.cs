namespace Domain.Entities;

public class Mecanico : BaseEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Especialidad { get; set; }
    public bool IsActive { get; set; } = true;

    public UserMember User { get; set; } = null!;
}
