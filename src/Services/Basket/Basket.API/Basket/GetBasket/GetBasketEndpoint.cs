namespace Basket.API.Basket.GetBasket;

using Entities;
using MediatR;

public record GetBasketResponse(Basket Basket);


public class GetBasketEndpoint : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}",
        async (string userName, ISender sender) => {
            var result = await sender.Send(new GetBasketQuery(userName));
            var response = result.Adapt<GetBasketResult>();

            return Results.Ok(response);
        });
    }

}
