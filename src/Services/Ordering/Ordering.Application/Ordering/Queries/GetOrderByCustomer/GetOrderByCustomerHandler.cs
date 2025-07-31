namespace Ordering.Application.Ordering.Queries.GetOrderByCustomer;

using BuildingBlocks.CQRS;
using Data;
using Extensions;
using Microsoft.EntityFrameworkCore;


public class GetOrderByCustomerHandler(IAppDbContext context) : IQueryHandler<GetOrderByCustomerQuery, GetOrderByCustomerResult> {

    public async Task<GetOrderByCustomerResult> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.CustomerId.Value == query.CustomerId)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync(cancellationToken);


        return new GetOrderByCustomerResult()
        {
            Orders = orders.ConvertToOrderDtos()
        };
    }

}
