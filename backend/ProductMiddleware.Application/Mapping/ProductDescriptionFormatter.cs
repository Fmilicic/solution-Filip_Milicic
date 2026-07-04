namespace ProductMiddleware.Application.Mapping;

public static class ProductDescriptionFormatter
{
    public const int DefaultMaxLength = 100;

    public static string Truncate(string? description, int maxLength = DefaultMaxLength)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return string.Empty;
        }

        return description.Length <= maxLength
            ? description
            : description[..maxLength];
    }
}
