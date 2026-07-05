using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ProductMiddleware.Application;

namespace ProductMiddleware.Tests.Integration;

[Collection("Integration")]
public class AdminProductsEndpointTests
{
    private readonly IntegrationWebApplicationFactory _factory;

    public AdminProductsEndpointTests(IntegrationWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreated_WithLocalId()
    {
        var client = _factory.CreateClient();
        var token = await LoginAndGetTokenAsync(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync("/api/admin/products", new
        {
            name = "Local Test Product",
            price = 29.99,
            description = "Created during integration test",
            imageUrl = "https://example.com/product.png",
            category = "gadgets"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.GetProperty("id").GetInt32() >= ProductIdConstants.LocalProductIdStart);
        Assert.Equal("Local Test Product", payload.GetProperty("name").GetString());
    }

    private static async Task<string> LoginAndGetTokenAsync(HttpClient client)
    {
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
        {
            username = "emilys",
            password = "emilyspass"
        });

        loginResponse.EnsureSuccessStatusCode();
        var payload = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
        return payload.GetProperty("accessToken").GetString()!;
    }
}
