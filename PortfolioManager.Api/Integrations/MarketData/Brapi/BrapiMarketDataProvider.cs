namespace PortfolioManager.Api.Integrations.MarketData.Brapi;

public class BrapiMarketDataProvider : IMarketDataProvider
{
    private readonly HttpClient _httpClient;

    public BrapiMarketDataProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<AssetPrice?> GetPriceAsync(
        string symbol,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}