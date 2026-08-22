using System.ComponentModel.DataAnnotations;
using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Dtos;

public class CreateInvestmentTransactionDto
{
    public int PortfolioId { get; set; }

    public int AssetId { get; set; }

    public TransactionType Type { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    public DateTime TransactionDate { get; set; }
}