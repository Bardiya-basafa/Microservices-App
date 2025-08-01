﻿namespace Catalog.API.Products.CreateProduct;


public  record CreateProductRequest(string Name, decimal Price, List<string> Categories, string Description, string ImageUrl) : IRequest<CreateProductResponse>;

public  record CreateProductResponse(Guid Id);


public class CreateProductEndpoint : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/product",
            async (CreateProductRequest request, ISender sender) => {
                var command = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/product/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Creates a new product");
    }

}
