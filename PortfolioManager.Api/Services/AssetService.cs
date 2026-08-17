using Microsoft.EntityFrameworkCore;
using PortfolioManager.Api.Data;
using PortfolioManager.Api.Models;

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
    string? search)
    {
        var query = _context.Assets
            .AsNoTracking()
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

    public async Task<Asset?> GetByIdAsync(int id)
    {
        return await _context.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(asset => asset.Id == id);
    }

    public async Task<List<Asset>> GetByPortfolioIdAsync(int portfolioId)
    {
        return await _context.Assets
            .AsNoTracking()
            .Where(asset => asset.PortfolioId == portfolioId)
            .ToListAsync();
    }

    public async Task<Asset?> CreateAsync(Asset asset)
    {
        var portfolioExists = await _context.Portfolios
            .AnyAsync(portfolio => portfolio.Id == asset.PortfolioId);

        if (!portfolioExists)
            return null;

        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        return asset;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var asset = await _context.Assets.FindAsync(id);

        if (asset is null)
            return false;

        _context.Assets.Remove(asset);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<(Asset? Asset, bool PortfolioExists)> UpdateAsync(
    int id,
    Asset updatedAsset)
    {
        var asset = await _context.Assets.FindAsync(id);

        if (asset is null)
            return (null, true);

        var portfolioExists = await _context.Portfolios
            .AnyAsync(portfolio => portfolio.Id == updatedAsset.PortfolioId);

        if (!portfolioExists)
            return (null, false);

        asset.Symbol = updatedAsset.Symbol;
        asset.Name = updatedAsset.Name;
        asset.Quantity = updatedAsset.Quantity;
        asset.PurchasePrice = updatedAsset.PurchasePrice;
        asset.PortfolioId = updatedAsset.PortfolioId;

        await _context.SaveChangesAsync();

        return (asset, true);
    }

    public async Task<int> CountAsync(string? search)
    {
        var query = _context.Assets.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(asset =>
                asset.Symbol.Contains(search) ||
                asset.Name.Contains(search));
        }

        return await query.CountAsync();
    }
}