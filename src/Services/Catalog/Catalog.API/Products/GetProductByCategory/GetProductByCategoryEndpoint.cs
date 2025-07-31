﻿namespace Catalog.API.Products.GetProductByCategory;

// public record GetProductByCategoryRequest(string Category) : IRequest<GetProductByCategoryResponse>;

public record GetProductByCategoryResponse(IEnumerable<Product> Products);


public class GetProductByCategoryEndpoint : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/product/category/{categotry}",
        async (string category, ISender sender) => {
            var result = await sender.Send(new GetProductByCategoryQuery(category));
            var response = result.Adapt<GetProductByCategoryResponse>();

            return Results.Ok(response);
        });
    }

}
