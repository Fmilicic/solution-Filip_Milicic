namespace ProductMiddleware.Application.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(int userId, string username);
}
