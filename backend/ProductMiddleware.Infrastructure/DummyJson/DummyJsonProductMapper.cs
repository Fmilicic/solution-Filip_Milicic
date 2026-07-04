using ProductMiddleware.Application.Models;
using ProductMiddleware.Infrastructure.DummyJson.Models;

namespace ProductMiddleware.Infrastructure.DummyJson;

public static class DummyJsonProductMapper
{
    public static Product ToDomain(DummyJsonProduct source)
    {
        return new Product
        {
            Id = source.Id,
            Name = source.Title,
            Price = source.Price,
            Description = source.Description,
            ImageUrl = source.Thumbnail,
            Category = source.Category
        };
    }
}
