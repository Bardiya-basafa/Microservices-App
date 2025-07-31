namespace Ordering.API.Endpoints;

using Application.Ordering.Commands.DeleteOrder;
using Carter;
using Mapster;
using MediatR;


public record DeleteOrderResponse {

    public bool IsSuccess { get; init; }

    

}


public class DeleteOrder : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{orderId}",
        async (Guid orderId, ISender sender) => {
            var command = new DeleteOrderCommand(orderId);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteOrderResponse>();

            return Results.Ok(response);
        });
    }

}
