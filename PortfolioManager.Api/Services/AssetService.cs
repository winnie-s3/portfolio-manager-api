using Microsoft.EntityFrameworkCore;
using PortfolioManager.Api.Data;
using PortfolioManager.Api.Models;
using PortfolioManager.Api.Exceptions;

namespace PortfolioManager.Api.Services;

public class AssetService : IAssetService
{
    private readonly AppDbContext _context;

    public AssetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Asset>> GetAllAsync(
    int page,
    int pageSize,
    string? search,
    int userId)
    {
        var query = _context.Assets
            .AsNoTracking()
            .Where(asset => asset.Portfolio.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(asset =>
                asset.Symbol.Contains(search) ||
                asset.Name.Contains(search));
        }

        return await query
            .OrderBy(asset => asset.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Asset?> GetByIdAsync(
    int id,
    int userId)
    {
        var asset = await _context.Assets
            .AsNoTracking()
            .Include(a => a.Portfolio)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (asset is null)
            return null;

        if (asset.Portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this asset.");

        return asset;
    }

    public async Task<List<Asset>> GetByPortfolioIdAsync(
    int portfolioId,
    int userId)
    {
        var portfolio = await _context.Portfolios
            .AsNoTracking()
            .FirstOrDefaultAsync(
                portfolio => portfolio.Id == portfolioId);

        if (portfolio is null)
            throw new ResourceNotFoundException(
                "Portfolio not found.");

        if (portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this portfolio.");

        return await _context.Assets
            .AsNoTracking()
            .Where(asset => asset.PortfolioId == portfolioId)
            .ToListAsync();
    }

    public async Task<Asset?> CreateAsync(
    Asset asset,
    int userId)
    {
        var portfolio = await _context.Portfolios
            .FirstOrDefaultAsync(
                portfolio => portfolio.Id == asset.PortfolioId);

        if (portfolio is null)
            return null;

        if (portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this portfolio.");

        _context.Assets.Add(asset);

        await _context.SaveChangesAsync();

        return asset;
    }

    public async Task<(Asset? Asset, bool PortfolioExists)> UpdateAsync(
    int id,
    Asset updatedAsset,
    int userId)
    {
        var asset = await _context.Assets
            .Include(a => a.Portfolio)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (asset is null)
            return (null, true);

        if (asset.Portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this asset.");

        var targetPortfolio = await _context.Portfolios
            .FirstOrDefaultAsync(
                p => p.Id == updatedAsset.PortfolioId);

        if (targetPortfolio is null)
            return (null, false);

        if (targetPortfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this portfolio.");

        asset.Symbol = updatedAsset.Symbol;
        asset.Name = updatedAsset.Name;
        asset.Quantity = updatedAsset.Quantity;
        asset.PurchasePrice = updatedAsset.PurchasePrice;
        asset.PortfolioId = updatedAsset.PortfolioId;

        await _context.SaveChangesAsync();

        return (asset, true);
    }

    public async Task<int> CountAsync(
    string? search,
    int userId)
    {
        var query = _context.Assets
            .Where(asset => asset.Portfolio.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(asset =>
                asset.Symbol.Contains(search) ||
                asset.Name.Contains(search));
        }

        return await query.CountAsync();
    }
}