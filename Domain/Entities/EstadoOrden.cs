using Domain.ValueObjects;

namespace Domain.Entities;

public class EstadoOrden
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
}
