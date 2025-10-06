namespace Domain.Entities;

public class Cliente : BaseEntity
{
    public int Id { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public bool IsActive { get; set; } = true;

    // Relación 1:1 con usuario
    public UserMember User { get; set; } = null!;
}
