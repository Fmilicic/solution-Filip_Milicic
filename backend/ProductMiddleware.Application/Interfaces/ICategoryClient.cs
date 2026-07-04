using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Interfaces;

public interface ICategoryClient
{
    Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
}
