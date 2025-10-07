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

    // esto es un metodo estático para crear un Id temporal (solo si se necesita en memoria antes de guardar)
    public static IdVO CreateNew()
    {
        // Por simplicidad, se genera un Id negativo temporal que luego será reemplazado por la BD
        return new IdVO(-1);
    }
}
