using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Services;

public interface IAssetService
{
    Task<List<Asset>> GetAllAsync(
    int page,
    int pageSize,
    string? search,
    int userId);

    Task<int> CountAsync(
        string? search,
        int userId);

    Task<Asset?> GetByIdAsync(
    int id,
    int userId);

    Task<List<Asset>> GetByPortfolioIdAsync(
    int portfolioId,
    int userId);

    Task<Asset?> CreateAsync(
    Asset asset,
    int userId);

    Task<(Asset? Asset, bool PortfolioExists)> UpdateAsync(
    int id,
    Asset asset,
    int userId);
}