using Microsoft.Extensions.Logging;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryClient _categoryClient;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryClient categoryClient, ILogger<CategoryService> logger)
    {
        _categoryClient = categoryClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching product categories");
        return await _categoryClient.GetCategoriesAsync(cancellationToken);
    }
}
