using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Services;

public interface IPortfolioService
{
    Task<List<Portfolio>> GetAllAsync();

    Task<Portfolio?> GetByIdAsync(int id);

    Task<Portfolio> CreateAsync(Portfolio portfolio);

    Task<bool> UpdateAsync(int id, Portfolio updatedPortfolio);

    Task<bool> DeleteAsync(int id);
}