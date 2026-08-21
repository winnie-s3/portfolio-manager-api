namespace PortfolioManager.Api.Integrations.MarketData.Brapi;

public class BrapiQuoteResponse
{
    public List<BrapiQuoteResult> Results { get; set; } = [];
}

public class BrapiQuoteResult
{
    public string Symbol { get; set; } = string.Empty;

    public decimal RegularMarketPrice { get; set; }
}