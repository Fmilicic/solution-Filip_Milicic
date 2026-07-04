using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Infrastructure.Sources;

public sealed class AggregatingProductSource : IProductSource
{
    private readonly IReadOnlyList<IProductSource> _sources;

    public AggregatingProductSource(IEnumerable<IProductSource> sources)
    {
        _sources = sources.ToList();
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        foreach (var source in _sources)
        {
            var product = await source.GetByIdAsync(id, cancellationToken);
            if (product is not null)
            {
                return product;
            }
        }

        return null;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var results = await Task.WhenAll(_sources.Select(source => source.GetAllAsync(cancellationToken)));
        return Merge(results);
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var results = await Task.WhenAll(_sources.Select(source => source.SearchAsync(query, cancellationToken)));
        return Merge(results);
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var results = await Task.WhenAll(_sources.Select(source => source.GetByCategoryAsync(category, cancellationToken)));
        return Merge(results);
    }

    private static IReadOnlyList<Product> Merge(IReadOnlyList<Product>[] sourceResults)
    {
        var productsById = new Dictionary<int, Product>();

        foreach (var sourceResult in sourceResults)
        {
            foreach (var product in sourceResult)
            {
                productsById.TryAdd(product.Id, product);
            }
        }

        return productsById.Values.OrderBy(product => product.Id).ToList();
    }
}
