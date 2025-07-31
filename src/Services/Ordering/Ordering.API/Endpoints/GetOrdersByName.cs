namespace Ordering.API.Endpoints;

using Application.DTOs;
using Application.Ordering.Queries.GetOrderByName;
using Carter;
using Mapster;
using MediatR;


public record GetOrdersByNameResponse {

    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

}


public class GetOrdersByName : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}",
        async (string orderName, ISender sender) => {
            var result = await sender.Send(new GetOrderByNameQuery()
            {
                Name = orderName
            });

            var response = result.Adapt<GetOrdersByNameResponse>();

            return Results.Ok(response);
        });
    }

}
