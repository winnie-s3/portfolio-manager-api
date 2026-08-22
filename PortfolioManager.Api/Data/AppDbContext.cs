using Microsoft.EntityFrameworkCore;
using PortfolioManager.Api.Models;

namespace PortfolioManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<InvestmentTransaction> InvestmentTransactions { get; set; }
}