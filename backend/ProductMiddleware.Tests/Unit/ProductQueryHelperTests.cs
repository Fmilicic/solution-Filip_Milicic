using ProductMiddleware.Application.Models;
using ProductMiddleware.Application.Services;

namespace ProductMiddleware.Tests.Unit;

public class ProductQueryHelperTests
{
    [Fact]
    public void Paginate_ReturnsRequestedPage()
    {
        var items = Enumerable.Range(1, 10)
            .Select(id => new Product { Id = id, Name = $"Product {id}" })
            .ToList();

        var result = ProductQueryHelper.Paginate(items, page: 2, pageSize: 3);

        Assert.Equal(3, result.Items.Count);
        Assert.Equal(4, result.Items[0].Id);
        Assert.Equal(2, result.Page);
        Assert.Equal(3, result.PageSize);
        Assert.Equal(10, result.Total);
    }

    [Fact]
    public void ApplyPriceFilter_ReturnsProductsWithinRange()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Price = 10 },
            new() { Id = 2, Price = 50 },
            new() { Id = 3, Price = 100 }
        };

        var result = ProductQueryHelper.ApplyPriceFilter(products, minPrice: 20, maxPrice: 80);

        Assert.Single(result);
        Assert.Equal(2, result[0].Id);
    }
}
