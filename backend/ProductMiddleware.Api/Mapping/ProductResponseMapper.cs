using ProductMiddleware.Api.Contracts;
using ProductMiddleware.Application.Mapping;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Api.Mapping;

public static class ProductResponseMapper
{
    public static ProductListItemResponse ToListItem(Product product)
    {
        return new ProductListItemResponse
        {
            Id = product.Id,
            Image = product.ImageUrl,
            Name = product.Name,
            Price = product.Price,
            ShortDescription = ProductDescriptionFormatter.Truncate(product.Description)
        };
    }

    public static ProductDetailResponse ToDetail(Product product)
    {
        return new ProductDetailResponse
        {
            Id = product.Id,
            Image = product.ImageUrl,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Category = product.Category
        };
    }

    public static PagedProductsResponse ToPagedResponse(PagedResult<Product> result)
    {
        return new PagedProductsResponse
        {
            Items = result.Items.Select(ToListItem).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            Total = result.Total
        };
    }
}
