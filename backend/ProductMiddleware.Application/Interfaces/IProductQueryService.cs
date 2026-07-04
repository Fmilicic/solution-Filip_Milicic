using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Interfaces;

public interface IProductQueryService
{
    Task<PagedResult<Product>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<Product>> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Product>> FilterAsync(string? category, decimal? minPrice, decimal? maxPrice, int page, int pageSize, CancellationToken cancellationToken = default);
}
