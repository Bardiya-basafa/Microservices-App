namespace Ordering.Application.Ordering.Commands.CreateOrder;

using BuildingBlocks.CQRS;
using Data;
using Domain.Entities;
using Domain.ValueObjects;
using DTOs;


public class CreateOrderHandler(IAppDbContext context) : ICommandHandler<CreateOrderCommand, CreateOrderResult> {

    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateOrder(command.Order);
        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id.Value);
    }

    private Order CreateOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.City, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.City, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

        var newOrder = Order.Create(
        id: OrderId.Of(Guid.NewGuid()),
        customerId: CustomerId.Of(orderDto.CustomerId),
        orderName: OrderName.Of(orderDto.OrderName),
        shippingAddress: shippingAddress,
        billingAddress: billingAddress,
        payment: Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.ExpirationDate, orderDto.Payment.CvvCode, orderDto.Payment.PaymentMethod),
        status: orderDto.Status);

        foreach (var orderItemDto in orderDto.OrderItems){
            newOrder.AddItem(ProductId.Of(orderItemDto.ProductId.Value), orderItemDto.Quantity, orderItemDto.Price);
        }

        return newOrder;
    }

}
