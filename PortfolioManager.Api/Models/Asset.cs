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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}