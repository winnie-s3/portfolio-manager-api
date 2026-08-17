using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Services;

public interface IAssetService
{
    Task<List<Asset>> GetAllAsync(
    int page,
    int pageSize,
    string? search);

    Task<Asset?> GetByIdAsync(int id);

    Task<List<Asset>> GetByPortfolioIdAsync(int portfolioId);

    Task<Asset?> CreateAsync(Asset asset);

    Task<bool> DeleteAsync(int id);
    Task<(Asset? Asset, bool PortfolioExists)> UpdateAsync(int id, Asset asset);
    Task<int> CountAsync(string? search);
}