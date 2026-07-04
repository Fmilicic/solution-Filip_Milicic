namespace ProductMiddleware.Api.Contracts;

public sealed class ProductListItemResponse
{
    public string Image { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string ShortDescription { get; init; } = string.Empty;
}
