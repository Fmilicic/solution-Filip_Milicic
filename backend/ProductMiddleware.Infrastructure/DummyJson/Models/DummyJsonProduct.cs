using System.Text.Json.Serialization;

namespace ProductMiddleware.Infrastructure.DummyJson.Models;

public sealed class DummyJsonProduct
{
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
}
