namespace ProductMiddleware.Api.Contracts;

public sealed class CategoryResponse
{
    public string Slug { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
}
