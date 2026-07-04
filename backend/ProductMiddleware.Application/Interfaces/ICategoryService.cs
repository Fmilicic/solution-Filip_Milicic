using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
}
