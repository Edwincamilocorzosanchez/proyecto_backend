namespace Domain.ValueObjects;

public record IdVO
{
    public int Value { get; }

    public IdVO(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El ID debe ser mayor que cero.");
        Value = value;
    }

    public override string ToString() => Value.ToString();
}
