using ProductMiddleware.Application;
using Microsoft.Extensions.Logging;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Services;

public sealed class ProductAdminService : IProductAdminService
{
    private readonly IProductRepository _repository;
    private readonly IProductQueryCacheInvalidator _cacheInvalidator;
    private readonly ILogger<ProductAdminService> _logger;

    public ProductAdminService(
        IProductRepository repository,
        IProductQueryCacheInvalidator cacheInvalidator,
        ILogger<ProductAdminService> logger)
    {
        _repository = repository;
        _cacheInvalidator = cacheInvalidator;
        _logger = logger;
    }

    public async Task<Product> CreateAsync(CreateProductData data, CancellationToken cancellationToken = default)
    {
        ValidateProductData(data.Name, data.Price);

        var product = await _repository.CreateAsync(data, cancellationToken);
        _cacheInvalidator.InvalidateSearchAndFilterCache();
        _logger.LogInformation("Created local product {ProductId}", product.Id);

        return product;
    }

    public async Task<Product?> UpdateAsync(int id, UpdateProductData data, CancellationToken cancellationToken = default)
    {
        if (id < ProductIdConstants.LocalProductIdStart)
        {
            _logger.LogWarning("Attempt to update external product {ProductId}", id);
            return null;
        }

        ValidateProductData(data.Name, data.Price);

        var product = await _repository.UpdateAsync(id, data, cancellationToken);
        if (product is not null)
        {
            _cacheInvalidator.InvalidateSearchAndFilterCache();
            _logger.LogInformation("Updated local product {ProductId}", id);
        }

        return product;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id < ProductIdConstants.LocalProductIdStart)
        {
            _logger.LogWarning("Attempt to delete external product {ProductId}", id);
            return false;
        }

        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            _cacheInvalidator.InvalidateSearchAndFilterCache();
            _logger.LogInformation("Deleted local product {ProductId}", id);
        }

        return deleted;
    }

    private static void ValidateProductData(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        }
    }
}
