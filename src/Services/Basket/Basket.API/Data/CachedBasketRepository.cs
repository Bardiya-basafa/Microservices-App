namespace Basket.API.Data;

using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Basket=global::Basket.API.Entities.Basket;
using JsonSerializer=System.Text.Json.JsonSerializer;


public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository {

    public async Task<Basket> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cashedBasket = await cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrWhiteSpace(cashedBasket)){
            return JsonSerializer.Deserialize<Basket>(cashedBasket)!;
        }

        var basket = await repository.GetBasket(userName, cancellationToken);
        await cache.SetStringAsync(userName, JsonConvert.SerializeObject(basket), cancellationToken);

        return basket;
    }

    public async Task<Basket> StoreBasket(Basket basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket), cancellationToken);

        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);

        return true;
    }

}
