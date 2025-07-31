namespace Ordering.Application.Data;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;


public interface IAppDbContext {

    DbSet<Customer> Customers { get; }

    DbSet<Order> Orders { get; }

    DbSet<OrderItem> OrderItems { get; }

    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
