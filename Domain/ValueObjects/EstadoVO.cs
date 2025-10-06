namespace Domain.ValueObjects;

public record EstadoVO
{
    public bool Value { get; }

    public EstadoVO(bool value)
    {
        if (value != true && value != false)
            throw new ArgumentException("El estado debe ser verdadero o falso.");
    }

    public bool EstaActivo() => Value;
    public override string ToString() => Value ? "Activo" : "Inactivo";
}
