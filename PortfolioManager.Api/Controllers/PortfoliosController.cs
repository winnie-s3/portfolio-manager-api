using Microsoft.AspNetCore.Mvc;
using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Models;
using PortfolioManager.Api.Services;
using PortfolioManager.Api.Mappings;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PortfolioManager.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PortfoliosController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;

    public PortfoliosController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PortfolioDto>>> GetAll()
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var portfolios =
            await _portfolioService.GetAllAsync(userId);

        var response = portfolios
            .Select(PortfolioMapper.ToDto)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PortfolioDto>> GetById(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var portfolio = await _portfolioService.GetByIdAsync(id, userId);

        if (portfolio is null)
            return NotFound();

        var response = PortfolioMapper.ToDto(portfolio);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<PortfolioDto>> Create(
    CreatePortfolioDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var portfolio = PortfolioMapper.ToEntity(request);

        portfolio.UserId = userId;

        var createdPortfolio =
            await _portfolioService.CreateAsync(portfolio);

        var response = PortfolioMapper.ToDto(createdPortfolio);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
    int id,
    UpdatePortfolioDto request)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var portfolio = PortfolioMapper.ToEntity(request);

        var updated =
            await _portfolioService.UpdateAsync(
                id,
                userId,
                portfolio);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var deleted =
            await _portfolioService.DeleteAsync(id, userId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}