using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Services;

public interface IUserService
{
    Task<User?> GetByEmailAsync(string email);

    Task<UserDto?> RegisterAsync(RegisterUserDto request);
}