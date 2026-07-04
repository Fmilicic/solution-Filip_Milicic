using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Services;

public static class ProductQueryHelper
{
    public static PagedResult<T> Paginate<T>(IReadOnlyList<T> items, int page, int pageSize)
    {
        var total = items.Count;
        var skip = (page - 1) * pageSize;
        var pageItems = items.Skip(skip).Take(pageSize).ToList();

        return new PagedResult<T>
        {
            Items = pageItems,
            Page = page,
            PageSize = pageSize,
            Total = total
        };
    }

    public static IReadOnlyList<Product> ApplyPriceFilter(IReadOnlyList<Product> products, decimal? minPrice, decimal? maxPrice)
    {
        IEnumerable<Product> filtered = products;

        if (minPrice is not null)
        {
            filtered = filtered.Where(product => product.Price >= minPrice.Value);
        }

        if (maxPrice is not null)
        {
            filtered = filtered.Where(product => product.Price <= maxPrice.Value);
        }

        return filtered.ToList();
    }
}
