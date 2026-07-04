using Microsoft.Extensions.Logging;
using ProductMiddleware.Application.Interfaces;
using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IAuthClient _authClient;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthClient authClient, ITokenService tokenService, ILogger<AuthService> logger)
    {
        _authClient = authClient;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<LoginResult?> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning("Login attempt with missing credentials");
            return null;
        }

        var user = await _authClient.LoginAsync(username, password, cancellationToken);
        if (user is null)
        {
            _logger.LogWarning("Login failed for user {Username}", username);
            return null;
        }

        var (token, expiresAt) = _tokenService.CreateToken(user.Id, user.Username);
        _logger.LogInformation("Login succeeded for user {Username}", username);

        return new LoginResult
        {
            AccessToken = token,
            ExpiresAt = expiresAt,
            Username = user.Username
        };
    }
}
