namespace Ordering.Application.Ordering.Commands.UpdateOrder;

using BuildingBlocks.CQRS;
using DTOs;
using FluentValidation;

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);


public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand> {

    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order).NotNull();
        RuleFor(x => x.Order.OrderName).NotEmpty();
    }

}
