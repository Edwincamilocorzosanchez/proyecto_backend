using Domain.ValueObjects;

namespace Domain.Entities;

public class TipoMovimiento
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
}
