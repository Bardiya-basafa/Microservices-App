namespace Ordering.Application.Ordering.EventHandler.Domain;

using Extensions;
using global::Ordering.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;


public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger, IPublishEndpoint publishEndpoint) : INotificationHandler<OrderUpdatedEvent> {

    public async Task Handle(OrderUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain event handled {DomainEvent}", domainEvent.GetType().Name);
        var orderCreatedIntegrationEvent = domainEvent.Order.ConvertToOrderDto();
        await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);

    }

}
