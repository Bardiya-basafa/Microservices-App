namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, List<string> Category, decimal Price, string Description, string ImageUrl) : IRequest<UpdateProductResponse>;

public record UpdateProductResponse(bool IsSuccess);


public class UpdateProductEndpoint : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products",
        async (UpdateProductRequest request, ISender sender) => {
            var command = request.Adapt<UpdateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductResponse>();

            return Results.Ok(response);
        });
    }

}
