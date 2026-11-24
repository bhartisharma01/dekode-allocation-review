using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private const string InvalidCredentials = "Invalid username or password.";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        TokenService tokenService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        // Unknown user aur galat password ka message same, taaki
        // username enumerate na ho sake.
        if (user is null)
        {
            _logger.LogWarning("Login failed: unknown user.");
            return Unauthorized(new { message = InvalidCredentials });
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("Login blocked: account locked. UserId={UserId}", user.Id);
            return Unauthorized(new
            {
                message = "Account is temporarily locked. Try again later."
            });
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            await _userManager.AccessFailedAsync(user); // lockout count badhta hai
            _logger.LogWarning("Login failed: bad password. UserId={UserId}", user.Id);
            return Unauthorized(new { message = InvalidCredentials });
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var (token, expiresAt) = _tokenService.GenerateToken(user, roles);

        _logger.LogInformation("Login succeeded. UserId={UserId}", user.Id);

        return Ok(new LoginResponseDto
        {
            Token = token,
            Username = user.UserName ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty,
            ExpiresAt = expiresAt
        });
    }
}