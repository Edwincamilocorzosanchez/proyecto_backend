namespace Domain.Entities;

public class Administrador : BaseEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? NivelAcceso { get; set; }
    public string? AreaResponsabilidad { get; set; }
    public bool IsActive { get; set; } = true;

    public UserMember User { get; set; } = null!;
}
