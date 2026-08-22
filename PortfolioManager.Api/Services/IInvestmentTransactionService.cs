using PortfolioManager.Api.Dtos;

namespace PortfolioManager.Api.Services;

public interface IInvestmentTransactionService
{
    Task<InvestmentTransactionDto> CreateAsync(
        CreateInvestmentTransactionDto dto,
        int userId,
        CancellationToken cancellationToken = default);

    Task<InvestmentTransactionDto?> GetByIdAsync(
    int id,
    int userId,
    CancellationToken cancellationToken = default);
}