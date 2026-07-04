using Microsoft.EntityFrameworkCore;
using ProductMiddleware.Application;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;
using ProductMiddleware.Infrastructure.Persistence.Entities;

namespace ProductMiddleware.Infrastructure.Persistence;

public sealed class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _dbContext;

    public ProductRepository(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(ProductEntityMapper.ToDomain).ToList();
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        return entity is null ? null : ProductEntityMapper.ToDomain(entity);
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var normalizedQuery = query.Trim().ToLowerInvariant();
        var entities = await _dbContext.Products
            .AsNoTracking()
            .Where(product => product.Name.ToLower().Contains(normalizedQuery))
            .OrderBy(product => product.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(ProductEntityMapper.ToDomain).ToList();
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var normalizedCategory = category.Trim().ToLowerInvariant();
        var entities = await _dbContext.Products
            .AsNoTracking()
            .Where(product => product.Category.ToLower() == normalizedCategory)
            .OrderBy(product => product.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(ProductEntityMapper.ToDomain).ToList();
    }

    public async Task<Product> CreateAsync(CreateProductData data, CancellationToken cancellationToken = default)
    {
        var maxId = await _dbContext.Products.MaxAsync(product => (int?)product.Id, cancellationToken)
            ?? ProductIdConstants.LocalProductIdStart - 1;
        var nextId = Math.Max(maxId + 1, ProductIdConstants.LocalProductIdStart);
        var now = DateTime.UtcNow;

        var entity = new ProductEntity
        {
            Id = nextId,
            Name = data.Name.Trim(),
            Price = data.Price,
            Description = data.Description.Trim(),
            ImageUrl = data.ImageUrl.Trim(),
            Category = data.Category.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.Products.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ProductEntityMapper.ToDomain(entity);
    }

    public async Task<Product?> UpdateAsync(int id, UpdateProductData data, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
        if (entity is null)
        {
            return null;
        }

        entity.Name = data.Name.Trim();
        entity.Price = data.Price;
        entity.Description = data.Description.Trim();
        entity.ImageUrl = data.ImageUrl.Trim();
        entity.Category = data.Category.Trim();
        entity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return ProductEntityMapper.ToDomain(entity);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        _dbContext.Products.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
