namespace Basket.API.Basket.DeleteBasket;

// public record DeleteBasketRequest(string UserName) : IRequest<DeleteBasketResponse>;

public record DeleteBasketResponse(bool BasketWasDeleted);


public class DeleteBasketEndpoint : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}",
        async (string userName, ISender sender) => {
            var result = await sender.Send(new DeleteBasketCommand(userName));
            var response = result.Adapt<DeleteBasketResponse>();

            return Results.Ok(response);
        });
    }

}
