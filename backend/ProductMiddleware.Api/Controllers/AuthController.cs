using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductMiddleware.Api.Contracts;
using ProductMiddleware.Application.Interfaces;

namespace ProductMiddleware.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiErrorResponse { Message = "Username and password are required." });
        }

        var result = await _authService.LoginAsync(request.Username, request.Password, cancellationToken);
        if (result is null)
        {
            return Unauthorized(new ApiErrorResponse { Message = "Invalid username or password." });
        }

        return Ok(new LoginResponse
        {
            AccessToken = result.AccessToken,
            ExpiresAt = result.ExpiresAt,
            Username = result.Username
        });
    }
}
