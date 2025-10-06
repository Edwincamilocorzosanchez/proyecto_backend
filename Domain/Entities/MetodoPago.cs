using Domain.ValueObjects;

namespace Domain.Entities;

public class MetodoPago
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
}
