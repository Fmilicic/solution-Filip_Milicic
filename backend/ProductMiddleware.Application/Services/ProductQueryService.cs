using Microsoft.Extensions.Logging;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Services;

public sealed class ProductQueryService : IProductQueryService
{
    private readonly IProductSource _productSource;
    private readonly ILogger<ProductQueryService> _logger;

    public ProductQueryService(IProductSource productSource, ILogger<ProductQueryService> logger)
    {
        _productSource = productSource;
        _logger = logger;
    }

    public async Task<PagedResult<Product>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching product list page {Page} size {PageSize}", page, pageSize);
        var products = await _productSource.GetAllAsync(cancellationToken);
        return ProductQueryHelper.Paginate(products, page, pageSize);
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching product details for id {ProductId}", id);
        return await _productSource.GetByIdAsync(id, cancellationToken);
    }

    public async Task<PagedResult<Product>> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching products with query {Query}", query);
        var products = await _productSource.SearchAsync(query, cancellationToken);
        return ProductQueryHelper.Paginate(products, page, pageSize);
    }

    public async Task<PagedResult<Product>> FilterAsync(string? category, decimal? minPrice, decimal? maxPrice, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Filtering products category {Category} min {MinPrice} max {MaxPrice}",
            category,
            minPrice,
            maxPrice);

        IReadOnlyList<Product> products;

        if (!string.IsNullOrWhiteSpace(category))
        {
            products = await _productSource.GetByCategoryAsync(category, cancellationToken);
        }
        else
        {
            products = await _productSource.GetAllAsync(cancellationToken);
        }

        products = ProductQueryHelper.ApplyPriceFilter(products, minPrice, maxPrice);
        return ProductQueryHelper.Paginate(products, page, pageSize);
    }
}
