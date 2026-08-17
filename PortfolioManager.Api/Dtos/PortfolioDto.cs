namespace PortfolioManager.Api.Dtos;

public class PortfolioDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string InvestorName { get; set; } = string.Empty;

    public decimal InitialBalance { get; set; }

    public DateTime CreatedAt { get; set; }
    public List<AssetDto> Assets { get; set; } = [];
    public decimal TotalValue { get; set; }
}