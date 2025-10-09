namespace Domain.ValueObjects;
public record IdVO : IEquatable<IdVO>
{
    public int Value { get; }

    public IdVO(int value)
    {
        // Permitir negativos y cero como IDs temporales generados por EF Core
        Value = value;
    }

    // Método para crear un ID temporal antes de guardar
    public static IdVO CreateNew() => new(0);

    public bool IsTemporary => Value <= 0;

    public override string ToString() => Value.ToString();
}
