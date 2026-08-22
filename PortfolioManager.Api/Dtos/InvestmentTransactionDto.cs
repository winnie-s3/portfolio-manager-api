using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Dtos;

public class InvestmentTransactionDto
{
    public int Id { get; set; }

    public int PortfolioId { get; set; }

    public int AssetId { get; set; }

    public TransactionType Type { get; set; }

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime CreatedAt { get; set; }
}