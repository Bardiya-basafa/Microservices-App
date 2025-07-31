namespace Ordering.Domain.Entities;

using Abstractions;
using ValueObjects;


public class Customer : Entity<CustomerId> {

    public string Name { get; private set; }

    public string Email { get; private set; }

    public static Customer Create(CustomerId id, string email, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        var customer = new Customer()
        {
            Id = id,
            Email = email,
            Name = name
        };

        return customer;
    }

}
