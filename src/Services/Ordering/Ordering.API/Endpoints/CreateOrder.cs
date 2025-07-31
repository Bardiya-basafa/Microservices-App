namespace Ordering.API.Endpoints;

using Application.DTOs;
using Application.Ordering.Commands.CreateOrder;
using Carter;
using Mapster;
using MediatR;


public record CreateOrderRequest {

    public OrderDto Order { get; init; }

}


public record CreateOrderResponse {

    public Guid OrderId { get; init; }

}


public class CreateOrder : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders",
        async (CreateOrderRequest request, ISender sender) => {
            var command = request.Adapt<CreateOrderCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateOrderResponse>();

            return Results.Created($"/orders/{response.OrderId}", response);
        });
    }

}
