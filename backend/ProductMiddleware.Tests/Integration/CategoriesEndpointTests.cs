using System.Net;

namespace ProductMiddleware.Tests.Integration;

[Collection("Integration")]
public class CategoriesEndpointTests
{
    private readonly IntegrationWebApplicationFactory _factory;

    public CategoriesEndpointTests(IntegrationWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCategories_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCategories_IncludesCorsHeader_ForFrontendOrigin()
    {
        var client = _factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/categories");
        request.Headers.Add("Origin", "http://localhost:5173");

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Headers.Contains("Access-Control-Allow-Origin"));
    }
}
