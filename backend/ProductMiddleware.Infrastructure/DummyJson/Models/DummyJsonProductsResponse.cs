namespace ProductMiddleware.Infrastructure.DummyJson.Models;

public sealed class DummyJsonProductsResponse
{
    public List<DummyJsonProduct> Products { get; set; } = new();
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}
