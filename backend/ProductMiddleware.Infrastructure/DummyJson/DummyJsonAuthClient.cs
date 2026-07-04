using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Infrastructure.DummyJson.Models;
using ProductMiddleware.Infrastructure.Options;

namespace ProductMiddleware.Infrastructure.DummyJson;

public sealed class DummyJsonAuthClient : IAuthClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DummyJsonAuthClient> _logger;

    public DummyJsonAuthClient(HttpClient httpClient, IOptions<DummyJsonOptions> options, ILogger<DummyJsonAuthClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }
    }

    public async Task<AuthUserResult?> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "auth/login",
            new { username, password },
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("DummyJSON login failed for user {Username} with status {StatusCode}", username, response.StatusCode);
            return null;
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<DummyJsonLoginResponse>(cancellationToken);
        if (loginResponse is null)
        {
            return null;
        }

        return new AuthUserResult
        {
            Id = loginResponse.Id,
            Username = loginResponse.Username
        };
    }
}
