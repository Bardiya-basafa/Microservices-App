namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);


public class GetProductByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult> {

    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {
        var product = await session.Query<Product>()
            .Where(p => p.Categories.Contains(request.Category, StringComparer.OrdinalIgnoreCase))
            .ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(product);
    }

}
