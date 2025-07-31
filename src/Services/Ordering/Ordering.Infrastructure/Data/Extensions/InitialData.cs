namespace Ordering.Infrastructure.Data.Extensions;

using Domain.Entities;
using Domain.ValueObjects;


public static class InitialData {

    public static IEnumerable<Customer> Customers()
    {
        return new List<Customer>()
        {
            Customer.Create(CustomerId.Of(new Guid()), "gmail@gmail.com", "User Name"),
            Customer.Create(CustomerId.Of(new Guid()), "Email@gmail.com", "User Name two"),
        };
    }

    public static IEnumerable<Product> Products()
    {
        return new List<Product>()
        {
            Product.Create(ProductId.Of(new Guid()), "Product one", 120),
            Product.Create(ProductId.Of(new Guid()), "Product two", 120),
        };
    }

    public static IEnumerable<Order> OrdersWithItems()
    {
        // from github 
        return new List<Order>();
    }

}
