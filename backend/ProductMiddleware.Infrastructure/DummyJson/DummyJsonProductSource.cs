using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Infrastructure.DummyJson;

public sealed class DummyJsonProductSource : IProductSource
{
    private const int FetchLimit = 0;

    private readonly DummyJsonProductClient _client;

    public DummyJsonProductSource(DummyJsonProductClient client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client.GetProductsAsync(FetchLimit, 0, cancellationToken);
        return response.Products.Select(DummyJsonProductMapper.ToDomain).ToList();
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _client.GetProductByIdAsync(id, cancellationToken);
            return DummyJsonProductMapper.ToDomain(product);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var response = await _client.SearchProductsAsync(query, cancellationToken);
        return response.Products.Select(DummyJsonProductMapper.ToDomain).ToList();
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetProductsByCategoryAsync(category, cancellationToken);
        return response.Products.Select(DummyJsonProductMapper.ToDomain).ToList();
    }
}
