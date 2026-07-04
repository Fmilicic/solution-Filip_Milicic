using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductMiddleware.Infrastructure.DummyJson.Models;
using ProductMiddleware.Infrastructure.Options;

namespace ProductMiddleware.Infrastructure.DummyJson;

public sealed class DummyJsonProductClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DummyJsonProductClient> _logger;

    public DummyJsonProductClient(HttpClient httpClient, IOptions<DummyJsonOptions> options, ILogger<DummyJsonProductClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }
    }

    public async Task<DummyJsonProductsResponse> GetProductsAsync(int limit, int skip, CancellationToken cancellationToken = default)
    {
        return await GetRequiredAsync<DummyJsonProductsResponse>($"products?limit={limit}&skip={skip}", cancellationToken);
    }

    public async Task<DummyJsonProduct> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetRequiredAsync<DummyJsonProduct>($"products/{id}", cancellationToken);
    }

    public async Task<DummyJsonProductsResponse> SearchProductsAsync(string query, CancellationToken cancellationToken = default)
    {
        var encodedQuery = Uri.EscapeDataString(query);
        return await GetRequiredAsync<DummyJsonProductsResponse>($"products/search?q={encodedQuery}", cancellationToken);
    }

    public async Task<DummyJsonProductsResponse> GetProductsByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var encodedCategory = Uri.EscapeDataString(category);
        return await GetRequiredAsync<DummyJsonProductsResponse>($"products/category/{encodedCategory}", cancellationToken);
    }

    private async Task<T> GetRequiredAsync<T>(string relativeUrl, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync(relativeUrl, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("DummyJSON request failed for {Url} with status {StatusCode}", relativeUrl, response.StatusCode);
            throw new HttpRequestException($"DummyJSON request failed with status code {(int)response.StatusCode}.");
        }

        var content = await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        if (content is null)
        {
            throw new InvalidOperationException($"DummyJSON returned empty content for {relativeUrl}.");
        }

        return content;
    }
}
