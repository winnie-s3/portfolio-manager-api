using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Mappings;

public static class PortfolioMapper
{
    public static PortfolioDto ToDto(Portfolio portfolio)
    {
        return new PortfolioDto
        {
            Id = portfolio.Id,
            Name = portfolio.Name,
            InvestorName = portfolio.InvestorName,
            InitialBalance = portfolio.InitialBalance,
            CreatedAt = portfolio.CreatedAt,
            TotalValue = portfolio.Assets
                .Sum(asset => asset.Quantity * asset.PurchasePrice),
            Assets = portfolio.Assets
                .Select(AssetMapper.ToDto)
                .ToList()
        };
    }

    public static Portfolio ToEntity(CreatePortfolioDto dto)
    {
        return new Portfolio
        {
            Name = dto.Name,
            InvestorName = dto.InvestorName,
            InitialBalance = dto.InitialBalance
        };
    }

    public static Portfolio ToEntity(UpdatePortfolioDto dto)
    {
        return new Portfolio
        {
            Name = dto.Name,
            InvestorName = dto.InvestorName,
            InitialBalance = dto.InitialBalance
        };
    }
}