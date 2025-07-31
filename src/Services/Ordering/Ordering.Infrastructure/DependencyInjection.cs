namespace Ordering.Infrastructure;

using Application.Data;
using Data;
using Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public static class DependencyInjection {

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<IAppDbContext, AppDbContext>();

        services.AddDbContext<AppDbContext>((serviceProvider, options) => {
            options.AddInterceptors(serviceProvider.GetService<ISaveChangesInterceptor>()!);
            options.UseSqlServer(connectionString);
        });

        return services;
    }

}
