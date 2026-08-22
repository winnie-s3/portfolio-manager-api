using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Services;
using System.Security.Claims;

namespace PortfolioManager.Api.Controllers;

[ApiController]
[Route("api/investment-transactions")]
[Authorize]
public class InvestmentTransactionsController : ControllerBase
{
    private readonly IInvestmentTransactionService _investmentTransactionService;

    public InvestmentTransactionsController(
        IInvestmentTransactionService investmentTransactionService)
    {
        _investmentTransactionService = investmentTransactionService;
    }

    [HttpPost]
    public async Task<ActionResult<InvestmentTransactionDto>> CreateAsync(
        CreateInvestmentTransactionDto dto,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var transaction = await _investmentTransactionService.CreateAsync(
            dto,
            userId,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = transaction.Id },
            transaction);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InvestmentTransactionDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var transaction = await _investmentTransactionService.GetByIdAsync(
            id,
            userId,
            cancellationToken);

        if (transaction is null)
            return NotFound();

        return Ok(transaction);
    }
}