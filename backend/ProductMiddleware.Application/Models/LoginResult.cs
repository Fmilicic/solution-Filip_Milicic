namespace ProductMiddleware.Application.Models;

public sealed class LoginResult
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
    public string Username { get; init; } = string.Empty;
}
