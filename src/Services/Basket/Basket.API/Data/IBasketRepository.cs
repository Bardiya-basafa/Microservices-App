namespace Basket.API.Data;

using Entities;


public interface IBasketRepository {

    Task<Basket> GetBasket(string userName, CancellationToken cancellationToken = default);

    Task<Basket> StoreBasket(Basket basket, CancellationToken cancellationToken = default);

    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);

}
