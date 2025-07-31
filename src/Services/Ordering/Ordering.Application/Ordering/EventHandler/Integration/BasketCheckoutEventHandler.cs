namespace Ordering.Application.Ordering.EventHandler.Integration;

using BuildingBlocks.Messaging.Events;
using Commands.CreateOrder;
using DTOs;
using global::Ordering.Domain.ValueObjects;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;


public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger) : IConsumer<BasketCheckoutEvent> {

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        logger.LogInformation("integration event handled: {integrationEvent}", context.Message.GetType().Name);
        var command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);
    }

    private static CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var addressDto = new AddressDto()
        {
        };

        var paymentDto = new PaymentDto()
        {
        };

        // return new CreateOrderCommand(new OrderDto(Guid.NewGuid(),Guid.NewGuid(), "ordername",addressDto, paymentDto));
        throw new NotImplementedException();
    }

}
