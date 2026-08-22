using Microsoft.EntityFrameworkCore;
using PortfolioManager.Api.Data;
using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Exceptions;
using PortfolioManager.Api.Mappings;

namespace PortfolioManager.Api.Services;

public class InvestmentTransactionService : IInvestmentTransactionService
{
    private readonly AppDbContext _context;

    public InvestmentTransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InvestmentTransactionDto> CreateAsync(
        CreateInvestmentTransactionDto dto,
        int userId,
        CancellationToken cancellationToken = default)
    {
        var portfolio = await _context.Portfolios
            .FirstOrDefaultAsync(
                p => p.Id == dto.PortfolioId,
                cancellationToken);

        if (portfolio is null)
            throw new ResourceNotFoundException("Portfolio not found.");

        if (portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this portfolio.");

        var assetExists = await _context.Assets
            .AnyAsync(
                a => a.Id == dto.AssetId,
                cancellationToken);

        if (!assetExists)
            throw new ResourceNotFoundException("Asset not found.");

        var transaction = InvestmentTransactionMapper.ToEntity(dto);

        _context.InvestmentTransactions.Add(transaction);

        await _context.SaveChangesAsync(cancellationToken);

        return InvestmentTransactionMapper.ToDto(transaction);
    }

    public async Task<InvestmentTransactionDto?> GetByIdAsync(
    int id,
    int userId,
    CancellationToken cancellationToken = default)
    {
        var transaction = await _context.InvestmentTransactions
            .Include(t => t.Portfolio)
            .FirstOrDefaultAsync(
                t => t.Id == id,
                cancellationToken);

        if (transaction is null)
            return null;

        if (transaction.Portfolio.UserId != userId)
            throw new ForbiddenAccessException(
                "You do not have access to this transaction.");

        return InvestmentTransactionMapper.ToDto(transaction);
    }
}