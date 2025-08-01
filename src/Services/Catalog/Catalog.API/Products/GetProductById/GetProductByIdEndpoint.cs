﻿namespace Catalog.API.Products.GetProductById;

using Entities;

// public record GetProductByIdRequest(Guid Id) : IRequest<GetProductByIdResponse>;

public record GetProductByIdResponse(Product Product);


public class GetProductByIdEndpoint : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}",
        async (Guid id, ISender sender) => {
            var result = await sender.Send(new GetProductByIdQuery(id));
            var response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(response);
        });
    }

}
