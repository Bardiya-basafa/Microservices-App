﻿namespace Ordering.Domain.ValueObjects;

public record OrderItemId {

    public Guid Value { get; init; }

    private OrderItemId(Guid value) => Value = value;

    public static OrderItemId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value == Guid.Empty){
            throw new ArgumentException("Value cannot be empty.", nameof(value));
        }

        return new OrderItemId(value);
    }

}
