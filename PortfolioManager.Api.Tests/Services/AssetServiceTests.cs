using Microsoft.EntityFrameworkCore;
using PortfolioManager.Api.Data;
using PortfolioManager.Api.Models;
using PortfolioManager.Api.Services;

namespace PortfolioManager.Api.Tests.Services;

public class AssetServiceTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNull_WhenPortfolioDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new AssetService(context);

        var asset = new Asset
        {
            Symbol = "PETR4",
            Name = "Petrobras",
            Quantity = 10,
            PurchasePrice = 30,
            PortfolioId = 999
        };

        // Act
        var result = await service.CreateAsync(asset);

        // Assert
        Assert.Null(result);
        Assert.Empty(context.Assets);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAsset_WhenPortfolioExists()
    {
        // Arrange
        await using var context = CreateContext();

        var portfolio = new Portfolio
        {
            Name = "Carteira Principal"
        };

        context.Portfolios.Add(portfolio);
        await context.SaveChangesAsync();

        var service = new AssetService(context);

        var asset = new Asset
        {
            Symbol = "PETR4",
            Name = "Petrobras",
            Quantity = 10,
            PurchasePrice = 30,
            PortfolioId = portfolio.Id
        };

        // Act
        var result = await service.CreateAsync(asset);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Single(context.Assets);
        Assert.Equal("PETR4", result.Symbol);
        Assert.Equal(portfolio.Id, result.PortfolioId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenAssetDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new AssetService(context);

        // Act
        var result = await service.DeleteAsync(999);

        // Assert
        Assert.False(result);
    }
}