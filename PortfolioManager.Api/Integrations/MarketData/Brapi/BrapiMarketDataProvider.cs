using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Json;
using PortfolioManager.Api.Exceptions;

namespace PortfolioManager.Api.Integrations.MarketData.Brapi;

public class BrapiMarketDataProvider : IMarketDataProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BrapiMarketDataProvider> _logger;

    public BrapiMarketDataProvider(
        HttpClient httpClient,
        ILogger<BrapiMarketDataProvider> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<AssetPrice?> GetPriceAsync(
    string symbol,
    CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
        "Fetching market price for asset {Symbol} using Brapi.",
        symbol);

        var response = await _httpClient.GetAsync(
            $"quote/{symbol}",
            cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation(
                "Brapi returned status code {StatusCode} for asset {Symbol}.",
                (int)response.StatusCode,
                symbol);
        }
        else
        {
            _logger.LogWarning(
                "Brapi returned non-success status code {StatusCode} for asset {Symbol}.",
                (int)response.StatusCode,
                symbol);
        }

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