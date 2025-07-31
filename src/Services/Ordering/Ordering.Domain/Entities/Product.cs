namespace Ordering.Domain.Entities;

using Abstractions;
using ValueObjects;


public class Product : Entity<ProductId> {

    public string Name { get; set; } = null!;

    public decimal Price { get; set; } = decimal.Zero;

    public static Product Create(ProductId id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

        var product = new Product()
        {
            Id = id,
            Name = name,
            Price = price
        };

        return product;
    }

}
