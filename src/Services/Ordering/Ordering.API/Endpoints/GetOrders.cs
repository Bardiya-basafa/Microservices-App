namespace Ordering.API.Endpoints;

using Application.DTOs;
using Application.Ordering.Queries.GetOrders;
using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;


public record GetOrdersResponse {

    public PaginatedResult<OrderDto> Orders { get; init; } = null!;

}


public class GetOrders : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders",
        async ([AsParameters] PaginationRequest request, ISender sender) => {
            var result = await sender.Send(new GetOrdersQuery()
            {
                PaginationRequest = request
            });

            var response = result.Adapt<GetOrdersResponse>();

            return Results.Ok(response);
        });
    }

}
