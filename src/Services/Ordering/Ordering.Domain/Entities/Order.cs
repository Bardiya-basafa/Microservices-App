namespace Ordering.Domain.Entities;

using Abstractions;
using Enums;
using Events;
using ValueObjects;


public class Order : Aggregate<OrderId> {

    private readonly List<OrderItem> _orderItems = new List<OrderItem>();

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public CustomerId CustomerId { get; private set; } = null!;

    public OrderName OrderName { get; private set; } = null!;

    public Address BillingAddress { get; private set; } = null!;

    public Address ShippingAddress { get; private set; } = null!;

    public Payment Payment { get; private set; } = null!;

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice
    {
        get => _orderItems.Sum(o => o.Price * o.Quantity);
        private set {}
    }

    public static Order Create(OrderId id, OrderName orderName, CustomerId customerId, Address billingAddress, Address shippingAddress, Payment payment, OrderStatus status)
    {
        var order = new Order()
        {
            Id = id,
            OrderName = orderName,
            BillingAddress = billingAddress,
            ShippingAddress = shippingAddress,
            CustomerId = customerId,
            Status = status,
            Payment = payment,
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));

        return order;
    }

    public void AddItem(ProductId productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var orderItem = new OrderItem(Id, productId, quantity, price);
        _orderItems.Add(orderItem);
    }

    public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
    {
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = status;

        AddDomainEvent(new OrderUpdatedEvent(this));
    }

}
