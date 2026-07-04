using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Infrastructure.Persistence;

public sealed class DatabaseProductSource : IProductSource
{
    private readonly IProductRepository _repository;

    public DatabaseProductSource(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(id, cancellationToken);

    public Task<IReadOnlyList<Product>> SearchAsync(string query, CancellationToken cancellationToken = default)
        => _repository.SearchAsync(query, cancellationToken);

    public Task<IReadOnlyList<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
        => _repository.GetByCategoryAsync(category, cancellationToken);
}
