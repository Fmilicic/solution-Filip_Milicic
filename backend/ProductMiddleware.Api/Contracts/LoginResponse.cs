namespace ProductMiddleware.Api.Contracts;

public sealed class LoginResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public string Username { get; init; } = string.Empty;
}
