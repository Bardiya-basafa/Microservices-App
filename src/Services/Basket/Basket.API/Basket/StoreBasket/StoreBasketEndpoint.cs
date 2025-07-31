namespace Basket.API.Basket.StoreBasket;

using Basket=global::Basket.API.Entities.Basket;

public record StoreBasketRequest(Basket Cart) : IRequest<StoreBasketResponse>;

public record StoreBasketResponse(string UserName);


public class StoreBasketEndpoint : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket",
        async (StoreBasketRequest request, ISender sender) => {
            var command = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(command);
            var response = new StoreBasketResponse(result.UserName);

            return Results.Created($"/basket/{response.UserName}", response);
        });
    }

}
