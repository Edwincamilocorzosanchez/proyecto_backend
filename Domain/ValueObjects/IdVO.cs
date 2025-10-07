namespace Domain.ValueObjects;

public record IdVO : IEquatable<IdVO>
{
        public Guid Value { get; }

        public IdVO(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("El ID no puede ser vacío.", nameof(value));

            Value = value;
        }

        public static IdVO NewId() => new(Guid.NewGuid());


        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();
}
