using Microsoft.AspNetCore.Mvc;
using PortfolioManager.Api.Integrations.MarketData;

namespace PortfolioManager.Api.Controllers;

[ApiController]
[Route("api/market-data")]
public class MarketDataController : ControllerBase
{
    private readonly IMarketDataProvider _marketDataProvider;

    public MarketDataController(IMarketDataProvider marketDataProvider)
    {
        _marketDataProvider = marketDataProvider;
    }

    [HttpGet("{symbol}")]
    public async Task<ActionResult<AssetPrice>> GetPriceAsync(
        string symbol,
        CancellationToken cancellationToken)
    {
        var assetPrice = await _marketDataProvider.GetPriceAsync(
            symbol,
            cancellationToken);

        if (assetPrice is null)
            return NotFound();

        return Ok(assetPrice);
    }
}