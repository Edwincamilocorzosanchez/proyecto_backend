using Domain.ValueObjects;

namespace Domain.Entities;

public class EstadoCita
{
    public IdVO Id { get; set; } = null!;
    public NombreVO Nombre { get; set; } = null!;
}