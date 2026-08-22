using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Mappings;

public static class InvestmentTransactionMapper
{
    public static InvestmentTransactionDto ToDto(
        InvestmentTransaction transaction)
    {
        return new InvestmentTransactionDto
        {
            Id = transaction.Id,
            PortfolioId = transaction.PortfolioId,
            AssetId = transaction.AssetId,
            Type = transaction.Type,
            Quantity = transaction.Quantity,
            UnitPrice = transaction.UnitPrice,
            TransactionDate = transaction.TransactionDate,
            CreatedAt = transaction.CreatedAt
        };
    }

    public static InvestmentTransaction ToEntity(
        CreateInvestmentTransactionDto dto)
    {
        return new InvestmentTransaction
        {
            PortfolioId = dto.PortfolioId,
            AssetId = dto.AssetId,
            Type = dto.Type,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            TransactionDate = dto.TransactionDate
        };
    }
}