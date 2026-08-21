using System.ComponentModel.DataAnnotations;

namespace PortfolioManager.Api.Models;

public class Asset
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Quantity { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal PurchasePrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int PortfolioId { get; set; }

    public Portfolio Portfolio { get; set; } = null!;
}