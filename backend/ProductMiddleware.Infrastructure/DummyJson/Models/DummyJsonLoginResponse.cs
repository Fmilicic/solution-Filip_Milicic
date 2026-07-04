using System.Text.Json.Serialization;

namespace ProductMiddleware.Infrastructure.DummyJson.Models;

public sealed class DummyJsonLoginResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;
}
