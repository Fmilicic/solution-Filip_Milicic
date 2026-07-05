using System.Text.Json.Serialization;

namespace ProductMiddleware.Infrastructure.DummyJson.Models;

public sealed class DummyJsonCategory
{
    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
