using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Services;

public interface IPortfolioService
{
    Task<List<Portfolio>> GetAllAsync(int userId);

    Task<Portfolio?> GetByIdAsync(
    int id,
    int userId);

    Task<Portfolio> CreateAsync(Portfolio portfolio);

    Task<bool> UpdateAsync(
    int id,
    int userId,
    Portfolio updatedPortfolio);

    Task<bool> DeleteAsync(
    int id,
    int userId);
}