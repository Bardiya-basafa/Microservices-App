namespace Ordering.API.Endpoints;

using Application.DTOs;
using Application.Ordering.Queries.GetOrderByCustomer;
using Carter;
using Mapster;
using MediatR;


public record GetOrdersByCustomerResponse {

    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

}


public class GetOrdersByCustomer : ICarterModule {

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId}",
        async (Guid customerId, ISender sender) => {
            var result = await sender.Send(new GetOrderByCustomerQuery()
            {
                CustomerId = customerId
            });

            var response = result.Adapt<GetOrdersByCustomerResponse>();

            return Results.Ok(response);
        });
    }

}
