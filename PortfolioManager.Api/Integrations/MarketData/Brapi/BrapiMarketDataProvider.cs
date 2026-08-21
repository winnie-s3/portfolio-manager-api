using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Json;
using PortfolioManager.Api.Exceptions;

namespace PortfolioManager.Api.Integrations.MarketData.Brapi;

public class BrapiMarketDataProvider : IMarketDataProvider
{
    private readonly HttpClient _httpClient;

    public BrapiMarketDataProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AssetPrice?> GetPriceAsync(
    string symbol,
    CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(
            $"quote/{symbol}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalServiceException(
                $"Market data provider returned status code {(int)response.StatusCode}.");
        }

        var brapiResponse =
            await response.Content.ReadFromJsonAsync<BrapiQuoteResponse>(
                cancellationToken);

        var quote = brapiResponse?.Results.FirstOrDefault();

        if (quote is null)
            return null;

        return new AssetPrice
        {
            Symbol = quote.Symbol,
            Price = quote.RegularMarketPrice,
            Provider = "Brapi",
            RetrievedAt = DateTime.UtcNow
        };
    }
}