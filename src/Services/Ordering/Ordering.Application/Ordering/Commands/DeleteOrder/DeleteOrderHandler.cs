namespace Ordering.Application.Ordering.Commands.DeleteOrder;

using BuildingBlocks.CQRS;
using Data;
using Domain.ValueObjects;
using Exceptions;
using Microsoft.EntityFrameworkCore;


public class DeleteOrderHandler(IAppDbContext context) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult> {

    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);
        var order = await context.Orders.FindAsync([orderId], cancellationToken);

        if (order == null){
            throw new OrderNotFoundException(orderId.Value);
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);
    }

}
