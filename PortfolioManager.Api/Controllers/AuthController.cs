using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PortfolioManager.Api.Configuration;
using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PortfolioManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserService _userService;

    public AuthController(
        IOptions<JwtSettings> jwtOptions,
        IUserService userService)
    {
        _jwtSettings = jwtOptions.Value;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
    LoginRequestDto request)
    {
        var normalizedEmail = request.Email.Trim().ToLower();

        var user = await _userService.GetByEmailAsync(normalizedEmail);

        if (user is null)
            return Unauthorized("E-mail ou senha inválidos.");

        var passwordIsValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordIsValid)
            return Unauthorized("E-mail ou senha inválidos.");

        var expiresAt = DateTime.UtcNow.AddMinutes(
            _jwtSettings.ExpirationInMinutes);

        var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.Name),
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.Role, user.Role)
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenString =
            new JwtSecurityTokenHandler().WriteToken(token);

        Response.Cookies.Append(
        "access_token",
        tokenString,
        new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Strict,
            Expires = expiresAt,
            Path = "/"
        });

        return Ok(new
        {
            expiresAt
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(
    RegisterUserDto request)
    {
        var user = await _userService.RegisterAsync(request);

        if (user is null)
            return Conflict("Já existe um usuário com esse e-mail.");

        return Created(string.Empty, user);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            id = userId,
            name,
            email
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            "access_token",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            });

        return NoContent();
    }
}