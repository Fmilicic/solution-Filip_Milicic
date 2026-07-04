using ProductMiddleware.Infrastructure.DummyJson;
using ProductMiddleware.Infrastructure.DummyJson.Models;

namespace ProductMiddleware.Tests.Unit;

public class DummyJsonProductMapperTests
{
    [Fact]
    public void ToDomain_MapsTitleToName()
    {
        var source = new DummyJsonProduct
        {
            Id = 1,
            Title = "iPhone 9",
            Description = "An apple mobile",
            Price = 549,
            Thumbnail = "https://cdn.dummyjson.com/product-thumbnail/1.jpg",
            Category = "smartphones"
        };

        var product = DummyJsonProductMapper.ToDomain(source);

        Assert.Equal("iPhone 9", product.Name);
        Assert.Equal("https://cdn.dummyjson.com/product-thumbnail/1.jpg", product.ImageUrl);
        Assert.Equal("smartphones", product.Category);
    }
}
