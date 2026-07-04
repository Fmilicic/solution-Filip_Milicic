namespace ProductMiddleware.Application.Interfaces;

public interface IAuthClient
{
    Task<AuthUserResult?> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}

public sealed class AuthUserResult
{
    public int Id { get; init; }
    public string Username { get; init; } = string.Empty;
}
