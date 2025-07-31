namespace Basket.API.Basket.GetBasket;

using Entities;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public record GetBasketResult(Basket Basket);


public class GetBasketHandler(IDocumentSession session, IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult> {

    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(query.UserName, cancellationToken);

        return new GetBasketResult(basket);
    }

}
