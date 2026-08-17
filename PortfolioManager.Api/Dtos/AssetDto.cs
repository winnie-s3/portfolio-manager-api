namespace PortfolioManager.Api.Dtos;

public class AssetDto
{
    public int Id { get; set; }

    public string Symbol { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal TotalValue { get; set; }

    public int PortfolioId { get; set; }

    public DateTime CreatedAt { get; set; }
}