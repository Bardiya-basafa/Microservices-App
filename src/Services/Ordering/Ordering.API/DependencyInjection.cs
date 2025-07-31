namespace Ordering.API;

using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


public static class DependencyInjection {

    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCarter();
        services.AddHealthChecks()
            .AddSqlServer(config.GetConnectionString("Database")!);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter();
        app.MapHealthChecks("/health", 
        new HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        

        return app;
    }

}
