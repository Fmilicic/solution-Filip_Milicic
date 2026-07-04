using ProductMiddleware.Application.Models;
using ProductMiddleware.Infrastructure.Persistence.Entities;

namespace ProductMiddleware.Infrastructure.Persistence;

internal static class ProductEntityMapper
{
    public static Product ToDomain(ProductEntity entity)
    {
        return new Product
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl,
            Category = entity.Category
        };
    }
}
