namespace Catalog.API.Products.CreateProduct;

using Entities;

public record CreateProductCommand(string Name, decimal Price, List<string> Categories, string Description, string ImageUrl) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);


public class CreateProductValidator : AbstractValidator<CreateProductCommand> {

    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage("Name must be between 0 and 50 characters.");
        RuleFor(x => x.Price).NotEmpty().InclusiveBetween(0, 3000).WithMessage("Price must be between 0 and 3000");
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255).WithMessage("Description must be between 0 and 255 characters.");
        RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(255).WithMessage("ImageUrl must be between 0 and 255 characters.");
    }

}


internal class CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult> {

    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(command, cancellationToken);
        var errors = result.Errors.Select(x => x.ErrorMessage).ToList();

        if (errors.Any()){
            throw new ValidationException(errors.FirstOrDefault());
        }

        var product = new Product()
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            ImageUrl = command.ImageUrl,
            Categories = command.Categories,
        };

        session.Store(product);
        await session.SaveChangesAsync(cancellationToken!);

        return new CreateProductResult(product.Id);
    }

}
