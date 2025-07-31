namespace Ordering.API.Endpoints;

using Application.DTOs;
using Application.Ordering.Commands.UpdateOrder;
using Azure.Core;
using Carter;
using Mapster;
using MediatR;


public record UpdateOrderRequest {

    public OrderDto Order { get; init; } = null!;

}


public class UpdateOrderResponse {

    public bool IsSuccess { get; init; }

}


public class UpdateOrder : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut($"/orders",
        async (UpdateOrderRequest request, ISender sender) => {
            var command = request.Adapt<UpdateOrderCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<OrderDto>();

            return Results.Ok(response);
        });
    }

}
