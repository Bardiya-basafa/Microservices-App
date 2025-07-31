namespace Ordering.Application.Ordering.Commands.UpdateOrder;

using BuildingBlocks.CQRS;
using Data;
using Domain.Entities;
using Domain.ValueObjects;
using DTOs;
using Exceptions;
using Microsoft.EntityFrameworkCore;


public class UpdateOrderHandler(IAppDbContext context) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult> {

    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.OrderId);
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order == null){
            throw new OrderNotFoundException(command.Order.OrderId);
        }

        UpdateOrder(order, command.Order);
        context.Orders.Update(order);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    private void UpdateOrder(Order order, OrderDto orderDto)
    {
        var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.City, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var updatedBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.City, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.ExpirationDate, orderDto.Payment.CvvCode, orderDto.Payment.PaymentMethod);

        order.Update(
        orderName: OrderName.Of(orderDto.OrderName),
        shippingAddress: updatedShippingAddress,
        billingAddress: updatedBillingAddress,
        payment: updatedPayment,
        status: orderDto.Status);
    }

}
