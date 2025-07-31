namespace BuildingBlocks.Pagination;

public record PaginatedResult<TEntity> where TEntity : class {

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public long Count { get; set; }

    public List<TEntity> Data { get; set; } = new List<TEntity>();

}
