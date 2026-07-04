namespace ProductMiddleware.Api.Contracts;

public sealed class PagedProductsResponse
{
    public IReadOnlyList<ProductListItemResponse> Items { get; init; } = Array.Empty<ProductListItemResponse>();
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int Total { get; init; }
}
