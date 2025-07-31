namespace Ordering.Domain.ValueObjects;

public record CustomerId {

    public Guid Value { get; init; }

    private CustomerId(Guid value) => Value = value;

    public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty){
            throw new ArgumentException("Value cannot be empty.", nameof(value));
        }

        return new CustomerId(value);
    }

}
