using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Mappings;

public static class AssetMapper
{
    public static AssetDto ToDto(Asset asset)
    {
        return new AssetDto
        {
            Id = asset.Id,
            Symbol = asset.Symbol,
            Name = asset.Name,
            Quantity = asset.Quantity,
            PurchasePrice = asset.PurchasePrice,
            TotalValue = asset.Quantity * asset.PurchasePrice,
            PortfolioId = asset.PortfolioId,
            CreatedAt = asset.CreatedAt
        };
    }

    public static Asset ToEntity(CreateAssetDto dto)
    {
        return new Asset
        {
            Symbol = dto.Symbol,
            Name = dto.Name,
            Quantity = dto.Quantity,
            PurchasePrice = dto.PurchasePrice,
            PortfolioId = dto.PortfolioId
        };
    }

    public static Asset ToEntity(UpdateAssetDto dto)
    {
        return new Asset
        {
            Symbol = dto.Symbol,
            Name = dto.Name,
            Quantity = dto.Quantity,
            PurchasePrice = dto.PurchasePrice,
            PortfolioId = dto.PortfolioId
        };
    }
}