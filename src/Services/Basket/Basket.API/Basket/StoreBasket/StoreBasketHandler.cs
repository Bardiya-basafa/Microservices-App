namespace Basket.API.Basket.StoreBasket;

using Discount.Grpc;
using Basket=global::Basket.API.Entities.Basket;

public record StoreBasketCommand(Basket Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);


public class StoreBasketHandler(DiscountProtoService.DiscountProtoServiceClient discountProto, IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult> {

    public async Task<StoreBasketResult> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Cart.Items){
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest() { ProductName = item.ProductName });
            item.Price -= decimal.Parse(coupon.Amount);
        }

        await repository.StoreBasket(request.Cart, cancellationToken);

        return new StoreBasketResult(request.Cart.UserName);
    }

}
