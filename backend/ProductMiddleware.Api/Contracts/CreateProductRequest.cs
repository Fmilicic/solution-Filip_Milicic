using System.ComponentModel.DataAnnotations;

namespace ProductMiddleware.Api.Contracts;

public sealed class CreateProductRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; init; }

    [MaxLength(4000)]
    public string Description { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; init; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; init; } = string.Empty;
}
