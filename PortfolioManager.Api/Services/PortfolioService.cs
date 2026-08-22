using Microsoft.EntityFrameworkCore;
using PortfolioManager.Api.Data;
using PortfolioManager.Api.Exceptions;
using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Services;

public class PortfolioService : IPortfolioService
{
    private readonly AppDbContext _context;

    public PortfolioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Portfolio>> GetAllAsync(int userId)
    {
        return await _context.Portfolios
            .AsNoTracking()
            .Include(portfolio => portfolio.Assets)
            .Where(portfolio => portfolio.UserId == userId)
            .ToListAsync();
    }

    public async Task<Portfolio?> GetByIdAsync(
    int id,
    int userId)
    {
        var portfolio = await _context.Portfolios
            .AsNoTracking()
            .Include(portfolio => portfolio.Assets)
            .FirstOrDefaultAsync(portfolio => portfolio.Id == id);

        if (portfolio is null)
            return null;

        if (portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this portfolio.");

        return portfolio;
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        _context.Portfolios.Add(portfolio);
        await _context.SaveChangesAsync();

        return portfolio;
    }

    public async Task<bool> UpdateAsync(
    int id,
    int userId,
    Portfolio updatedPortfolio)
    {
        var portfolio = await _context.Portfolios.FindAsync(id);

        if (portfolio is null)
            return false;

        if (portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this portfolio.");

        portfolio.Name = updatedPortfolio.Name;
        portfolio.InvestorName = updatedPortfolio.InvestorName;
        portfolio.InitialBalance = updatedPortfolio.InitialBalance;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(
    int id,
    int userId)
    {
        var portfolio = await _context.Portfolios.FindAsync(id);

        if (portfolio is null)
            return false;

        if (portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this portfolio.");

        _context.Portfolios.Remove(portfolio);
        await _context.SaveChangesAsync();

        return true;
    }
}