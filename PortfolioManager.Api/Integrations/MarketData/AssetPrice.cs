namespace PortfolioManager.Api.Integrations.MarketData
{
    public class AssetPrice
    {
        public string Symbol { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Provider { get; set; } = string.Empty;

        public DateTime RetrievedAt { get; set; }
    }
}
