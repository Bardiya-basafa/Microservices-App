﻿namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);


public class DeleteBasketCommandHandler(IDocumentSession session, IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult> {

    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await repository.DeleteBasket(command.UserName, cancellationToken);

        return new DeleteBasketResult(result);
    }

}
