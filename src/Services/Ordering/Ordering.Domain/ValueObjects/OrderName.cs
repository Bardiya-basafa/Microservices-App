namespace Ordering.Domain.ValueObjects;

public record OrderName {

    private const int DefaultLength = 5;

    public string Value { get; }

    public OrderName(string value) => Value = value;

    public static OrderName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        return new OrderName(value);
    }

}
