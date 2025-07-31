namespace Ordering.Application.Extensions;

using Domain.Entities;
using DTOs;


public static class OrderExtensions {

    public static List<OrderDto> ConvertToOrderDtos(this List<Order> orders)
    {
        // return orders.Select(order => new OrderDto(
        // OrderId: order.Id.Value,
        // CustomerId: order.CustomerId.Value,
        // OrderName: order.OrderName.Value,
        // Status: order.Status,
        //
        // ))

        throw new NotImplementedException();
    }

    public static OrderDto ConvertToOrderDto(this Order order)
    {
        throw new NotImplementedException();
    }

}
