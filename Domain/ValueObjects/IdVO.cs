namespace Domain.ValueObjects;

public record IdVO : IEquatable<IdVO>
{
    public int Value { get; }

    public IdVO(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El ID debe ser mayor que cero.");
        Value = value;
    }

    // Método para crear un ID temporal antes de guardar
    public static IdVO CreateNew() => new(-1);

    public override string ToString() => Value.ToString();

    public override int GetHashCode() => Value.GetHashCode();
}
