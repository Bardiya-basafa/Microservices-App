namespace Ordering.Application.DTOs;

using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;


public record OrderDto(
    Guid OrderId,
    Guid CustomerId,
    string OrderName,
    Address BillingAddress,
    Address ShippingAddress,
    Payment Payment,
    OrderStatus Status,
    List<OrderItem> OrderItems
);
