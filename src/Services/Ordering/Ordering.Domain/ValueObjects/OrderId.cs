namespace Ordering.Domain.ValueObjects;

public record OrderId {

    public Guid Value { get; init; }

    private OrderId(Guid value) => Value = value;

    public static OrderId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty){
            throw new ArgumentException("Value cannot be empty.", nameof(value));
        }

        return new OrderId(value);
    }

}
