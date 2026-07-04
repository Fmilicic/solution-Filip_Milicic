using ProductMiddleware.Application.Models;

namespace ProductMiddleware.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResult?> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}
