using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Services;

public sealed class CachedProductQueryService : IProductQueryService, IProductQueryCacheInvalidator
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(3);

    private readonly ProductQueryService _inner;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedProductQueryService> _logger;

    public CachedProductQueryService(
        ProductQueryService inner,
        IMemoryCache cache,
        ILogger<CachedProductQueryService> logger)
    {
        _inner = inner;
        _cache = cache;
        _logger = logger;
    }

    public Task<PagedResult<Product>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => _inner.GetListAsync(page, pageSize, cancellationToken);

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _inner.GetByIdAsync(id, cancellationToken);

    public async Task<PagedResult<Product>> SearchAsync(
        string query,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = BuildSearchKey(query, page, pageSize);

        if (_cache.TryGetValue(cacheKey, out PagedResult<Product>? cached) && cached is not null)
        {
            _logger.LogInformation("Cache hit for product search key {CacheKey}", cacheKey);
            return cached;
        }

        _logger.LogInformation("Cache miss for product search key {CacheKey}", cacheKey);
        var result = await _inner.SearchAsync(query, page, pageSize, cancellationToken);
        _cache.Set(cacheKey, result, CacheDuration);
        return result;
    }

    public async Task<PagedResult<Product>> FilterAsync(
        string? category,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = BuildFilterKey(category, minPrice, maxPrice, page, pageSize);

        if (_cache.TryGetValue(cacheKey, out PagedResult<Product>? cached) && cached is not null)
        {
            _logger.LogInformation("Cache hit for product filter key {CacheKey}", cacheKey);
            return cached;
        }

        _logger.LogInformation("Cache miss for product filter key {CacheKey}", cacheKey);
        var result = await _inner.FilterAsync(category, minPrice, maxPrice, page, pageSize, cancellationToken);
        _cache.Set(cacheKey, result, CacheDuration);
        return result;
    }

    public void InvalidateSearchAndFilterCache()
    {
        if (_cache is MemoryCache memoryCache)
        {
            memoryCache.Compact(1.0);
            _logger.LogInformation("Invalidated search and filter cache entries");
        }
    }

    internal static string BuildSearchKey(string query, int page, int pageSize)
        => $"product-search:{NormalizeText(query)}:{page}:{pageSize}";

    internal static string BuildFilterKey(string? category, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        => $"product-filter:{NormalizeText(category)}:{minPrice}:{maxPrice}:{page}:{pageSize}";

    private static string NormalizeText(string? value)
        => string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToLowerInvariant();
}
