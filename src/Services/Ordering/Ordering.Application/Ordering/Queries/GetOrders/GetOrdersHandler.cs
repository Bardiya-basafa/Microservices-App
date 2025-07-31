namespace Ordering.Application.Ordering.Queries.GetOrders;

using BuildingBlocks.CQRS;
using BuildingBlocks.Pagination;
using Data;
using DTOs;
using Extensions;
using Microsoft.EntityFrameworkCore;


public class GetOrdersQueryHandler(IAppDbContext context) : IQueryHandler<GetOrdersQuery, GetOrdersResult> {

    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedBy)
            .Skip(query.PaginationRequest.PageIndex * query.PaginationRequest.PageSize)
            .Take(query.PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await context.Orders.LongCountAsync(cancellationToken);

        return new GetOrdersResult()
        {
            Orders = new PaginatedResult<OrderDto>()
            {
                Count = totalCount,
                PageIndex = query.PaginationRequest.PageIndex,
                PageSize = query.PaginationRequest.PageSize,
                Data = orders.ConvertToOrderDtos()
            }
        };
    }

}
