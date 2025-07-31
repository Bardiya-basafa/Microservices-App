namespace Discount.Grpc.Data;

using Microsoft.EntityFrameworkCore;



public static class Extensions {

    public static IApplicationBuilder UseMigration(this IApplicationBuilder builder)
    {
        using var serviceScope = builder.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.MigrateAsync();

        return builder;
    }

}
