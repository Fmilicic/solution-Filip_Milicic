namespace ProductMiddleware.Application.Validation;

public static class ProductQueryValidator
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 50;

    public static string? ValidatePaging(int page, int pageSize)
    {
        if (page < 1)
        {
            return "page must be at least 1.";
        }

        if (pageSize < 1)
        {
            return "pageSize must be at least 1.";
        }

        if (pageSize > MaxPageSize)
        {
            return $"pageSize cannot exceed {MaxPageSize}.";
        }

        return null;
    }

    public static string? ValidateSearchQuery(string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return "query is required.";
        }

        return null;
    }

    public static string? ValidateFilter(string? category, decimal? minPrice, decimal? maxPrice)
    {
        if (string.IsNullOrWhiteSpace(category) && minPrice is null && maxPrice is null)
        {
            return "At least one filter parameter is required.";
        }

        if (minPrice is < 0)
        {
            return "minPrice cannot be negative.";
        }

        if (maxPrice is < 0)
        {
            return "maxPrice cannot be negative.";
        }

        if (minPrice is not null && maxPrice is not null && minPrice > maxPrice)
        {
            return "minPrice cannot exceed maxPrice.";
        }

        return null;
    }

    public static string? ValidateCreateProduct(string name, decimal price, string description, string imageUrl, string category)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "name is required.";
        }

        if (price < 0)
        {
            return "price cannot be negative.";
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return "description is required.";
        }

        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return "imageUrl is required.";
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            return "category is required.";
        }

        return null;
    }
}
