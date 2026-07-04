using System.Net;

namespace ProductMiddleware.Tests.Integration;

[Collection("Integration")]
public class ProductsListEndpointTests
{
    private readonly IntegrationWebApplicationFactory _factory;

    public ProductsListEndpointTests(IntegrationWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetProducts_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProductById_ReturnsOk_ForExistingProduct()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/products/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
