namespace Basket.API.Basket.CheckoutBasket;

using DTOs;

public record CheckoutBasketRequest(CheckoutBasketDto CheckoutBasketDto);

public record CheckoutBasketResponse(bool IsSuccess);


public class CheckoutBasketEndpoint : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout",
        async (CheckoutBasketDto request, ISender sender) => {
            var result = await sender.Send(new CheckoutBasketCommand(request));
            var response = result.Adapt<CheckoutBasketResponse>();

            return Results.Ok(response);
        });
    }

}
