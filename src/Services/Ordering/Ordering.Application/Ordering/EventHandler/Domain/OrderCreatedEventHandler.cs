namespace Ordering.Application.Ordering.EventHandler.Domain;

using global::Ordering.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;


public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent> {

    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain event Handled {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }

}
