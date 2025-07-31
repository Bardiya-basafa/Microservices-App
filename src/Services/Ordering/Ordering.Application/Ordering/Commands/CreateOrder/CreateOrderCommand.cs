namespace Ordering.Application.Ordering.Commands.CreateOrder;

using System.Windows.Input;
using BuildingBlocks.CQRS;
using DTOs;
using FluentValidation;

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);


public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand> {

    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order).NotNull();
        RuleFor(x => x.Order.OrderName).NotEmpty();
        RuleFor(x => x.Order.ShippingAddress).NotNull();

        throw new NotImplementedException();
    }

}
