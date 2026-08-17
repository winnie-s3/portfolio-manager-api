using System.ComponentModel.DataAnnotations;

namespace PortfolioManager.Api.Dtos;

public class UpdatePortfolioDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string InvestorName { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal InitialBalance { get; set; }
}