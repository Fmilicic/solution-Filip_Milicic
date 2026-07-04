using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProductMiddleware.Tests.Integration;

[Collection("Integration")]
public class AuthEndpointTests
{
    private readonly IntegrationWebApplicationFactory _factory;

    public AuthEndpointTests(IntegrationWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task SearchWithoutToken_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/products/search?query=phone");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsToken_WithValidCredentials()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            username = "emilys",
            password = "emilyspass"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(payload.TryGetProperty("accessToken", out var token));
        Assert.False(string.IsNullOrWhiteSpace(token.GetString()));
    }

    [Fact]
    public async Task SearchWithBlankQuery_ReturnsBadRequest_WhenAuthenticated()
    {
        var client = _factory.CreateClient();
        var token = await LoginAndGetTokenAsync(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync("/api/products/search?query=");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SearchWithToken_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var token = await LoginAndGetTokenAsync(client);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync("/api/products/search?query=phone");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
