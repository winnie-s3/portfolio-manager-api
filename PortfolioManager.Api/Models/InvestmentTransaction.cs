using System.ComponentModel.DataAnnotations;

namespace PortfolioManager.Api.Models;

public class InvestmentTransaction
{
    public int Id { get; set; }

    public int PortfolioId { get; set; }

    public Portfolio Portfolio { get; set; } = null!;

    public int AssetId { get; set; }

    public Asset Asset { get; set; } = null!;

    public TransactionType Type { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}