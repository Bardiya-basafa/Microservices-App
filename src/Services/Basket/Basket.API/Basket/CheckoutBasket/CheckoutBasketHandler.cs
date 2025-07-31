namespace Basket.API.Basket.CheckoutBasket;

using BuildingBlocks.Messaging.Events;
using DTOs;
using MassTransit;

public record CheckoutBasketCommand(CheckoutBasketDto CheckoutBasket) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);


public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand> {

    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.CheckoutBasket).NotNull().WithMessage("Please provide a valid CheckoutBasket.");
        RuleFor(x => x.CheckoutBasket.UserName).NotNull().NotEmpty().WithMessage("Please provide a valid UserName.");
    }

}


public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult> {

    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(command.CheckoutBasket.UserName, cancellationToken);

        if (basket == null){
            return new CheckoutBasketResult(false);
        }

        var eventMessage = command.CheckoutBasket.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        await publishEndpoint.Publish(eventMessage, cancellationToken);
        await repository.DeleteBasket(command.CheckoutBasket.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }

}
