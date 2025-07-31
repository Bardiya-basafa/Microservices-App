namespace Ordering.Application.Ordering.Queries.GetOrderByName;

using BuildingBlocks.CQRS;
using Data;
using Domain.Entities;
using DTOs;
using Extensions;
using Microsoft.EntityFrameworkCore;


public class GetOrderByNameQueryHandler(IAppDbContext context) : IQueryHandler<GetOrderByNameQuery, GetOrderByNameResult> {

    public async Task<GetOrderByNameResult> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.OrderName.Value.Contains(query.Name))
            .OrderBy(o => o.OrderName)
            .ToListAsync(cancellationToken);

        var orderDtos = ConvertToOrderDto(orders);

        return new GetOrderByNameResult()
        {
            Orders = orderDtos
        };
    }

    private List<OrderDto> ConvertToOrderDto(List<Order> orders)
    {
        var orderDtos = new List<OrderDto>();

        // actual logic

        return orderDtos;
    }

}
