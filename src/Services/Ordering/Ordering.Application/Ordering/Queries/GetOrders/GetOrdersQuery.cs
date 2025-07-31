namespace Ordering.Application.Ordering.Queries.GetOrders;

using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using DTOs;


public record GetOrdersQuery : IQuery<GetOrdersResult> {

    public PaginationRequest PaginationRequest { get; init; } = null!;

}


public record GetOrdersResult {

    public PaginatedResult<OrderDto> Orders { get; init; } = null!;

}
