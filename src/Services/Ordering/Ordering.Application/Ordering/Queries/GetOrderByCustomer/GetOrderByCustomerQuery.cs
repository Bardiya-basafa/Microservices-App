namespace Ordering.Application.Ordering.Queries.GetOrderByCustomer;

using BuildingBlocks.CQRS;
using DTOs;


public record GetOrderByCustomerQuery : IQuery<GetOrderByCustomerResult> {

    public Guid CustomerId { get; set; }

}


public record GetOrderByCustomerResult {

    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

}
