namespace PortfolioManager.Api.Integrations.MarketData;

public interface IMarketDataProvider
{
    Task<AssetPrice?> GetPriceAsync(
        string symbol,
        CancellationToken cancellationToken = default
    );
}