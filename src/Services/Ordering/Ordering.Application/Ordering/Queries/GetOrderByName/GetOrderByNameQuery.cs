namespace Ordering.Application.Ordering.Queries.GetOrderByName;

using BuildingBlocks.CQRS;
using Domain.Entities;
using DTOs;


public record GetOrderByNameQuery : IQuery<GetOrderByNameResult> {

    public string Name { get; set; } = string.Empty;

};


public record GetOrderByNameResult {

    public List<OrderDto> Orders { get; set; } = new List<OrderDto>();

}
