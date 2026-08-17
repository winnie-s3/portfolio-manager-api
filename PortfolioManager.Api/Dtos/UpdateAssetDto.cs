using System.ComponentModel.DataAnnotations;

namespace PortfolioManager.Api.Dtos;

public class UpdateAssetDto
{
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

    [Range(1, int.MaxValue)]
    public int PortfolioId { get; set; }
}