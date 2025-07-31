namespace Ordering.Domain.ValueObjects;

public record ProductId {

    public Guid Value { get; init; }

    private ProductId(Guid value) => Value = value;

    public static ProductId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty){
            throw new ArgumentException("Value cannot be empty.", nameof(value));
        }

        return new ProductId(value);
    }

}
