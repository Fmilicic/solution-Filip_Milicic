using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Interfaces;

public interface IProductAdminService
{
    Task<Product> CreateAsync(CreateProductData data, CancellationToken cancellationToken = default);
    Task<Product?> UpdateAsync(int id, UpdateProductData data, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
