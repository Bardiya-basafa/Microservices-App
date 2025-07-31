namespace Ordering.Infrastructure.Data.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


public static class DatabaseExtensions {

    public static async Task InitializeDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await Seed(context);
    }

    private static async Task Seed(AppDbContext context)
    {
        await SeedCustomer(context);
        await SeedProduct(context);
        await SeedOrderWithItems(context);
    }

    private static async Task SeedCustomer(AppDbContext context)
    {
        if (!await context.Customers.AnyAsync()){
            await context.Customers.AddRangeAsync(InitialData.Customers());
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedProduct(AppDbContext context)
    {
        if (!await context.Products.AnyAsync()){
            await context.Products.AddRangeAsync(InitialData.Products());
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOrderWithItems(AppDbContext context)
    {
        if (!await context.Orders.AnyAsync()){
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems());
            await context.SaveChangesAsync();
        }
    }

}
