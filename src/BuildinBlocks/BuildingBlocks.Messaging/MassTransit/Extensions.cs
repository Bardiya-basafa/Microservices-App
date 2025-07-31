namespace BuildingBlocks.Messaging.MassTransit;

using System.Reflection;
using global::MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public static class Extensions {

    public static IServiceCollection AddMessagingBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(config => {
            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null){
                config.AddConsumers(assembly);
            }

            config.UsingRabbitMq((context, configurator) => {
                configurator.Host(new Uri(configuration["MessagingBroker:Host"]!),
                host => {
                    host.Username(configuration["MessagingBroker:Username"]!);
                    host.Password(configuration["MessagingBroker:Password"]!);
                });

                configurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }

}
