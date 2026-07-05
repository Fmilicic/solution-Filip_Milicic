using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;
using ProductMiddleware.Infrastructure.DummyJson.Models;
using ProductMiddleware.Infrastructure.Options;

namespace ProductMiddleware.Infrastructure.DummyJson;

public sealed class DummyJsonCategoryClient : ICategoryClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DummyJsonCategoryClient> _logger;

    public DummyJsonCategoryClient(HttpClient httpClient, IOptions<DummyJsonOptions> options, ILogger<DummyJsonCategoryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
        }
    }

    public async Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("products/categories", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("DummyJSON categories request failed with status {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"DummyJSON request failed with status code {(int)response.StatusCode}.");
        }

        var categories = await response.Content.ReadFromJsonAsync<List<DummyJsonCategory>>(cancellationToken);
        if (categories is null)
        {
            throw new InvalidOperationException("DummyJSON returned empty categories content.");
        }

        return categories
            .Select(category => new Category
            {
                Slug = category.Slug,
                Name = category.Name
            })
            .ToList();
    }
}
