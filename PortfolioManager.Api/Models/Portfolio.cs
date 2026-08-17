namespace PortfolioManager.Api.Models;

public class Portfolio
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal InitialBalance { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}