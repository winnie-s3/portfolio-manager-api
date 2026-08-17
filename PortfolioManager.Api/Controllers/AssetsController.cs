using Microsoft.AspNetCore.Mvc;
using PortfolioManager.Api.Dtos;
using PortfolioManager.Api.Mappings;
using PortfolioManager.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace PortfolioManager.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AssetsController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetsController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<AssetDto>>> GetAll(
    int page = 1,
    int pageSize = 10,
    string? search = null)
    {
        if (page < 1)
            return BadRequest("A página deve ser maior que zero.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("O tamanho da página deve estar entre 1 e 100.");

        var assets = await _assetService.GetAllAsync(page, pageSize, search);

        var totalItems = await _assetService.CountAsync(search);

        var response = new PagedResponse<AssetDto>
        {
            Data = assets
                .Select(AssetMapper.ToDto)
                .ToList(),

            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
        };

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AssetDto>> GetById(int id)
    {
        var asset = await _assetService.GetByIdAsync(id);

        if (asset is null)
            return NotFound();

        return Ok(AssetMapper.ToDto(asset));
    }

    [HttpGet("portfolio/{portfolioId:int}")]
    public async Task<ActionResult<List<AssetDto>>> GetByPortfolio(
        int portfolioId)
    {
        var assets =
            await _assetService.GetByPortfolioIdAsync(portfolioId);

        var response = assets
            .Select(AssetMapper.ToDto)
            .ToList();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<AssetDto>> Create(
        CreateAssetDto request)
    {
        var asset = AssetMapper.ToEntity(request);

        var createdAsset = await _assetService.CreateAsync(asset);

        if (createdAsset is null)
        {
            return BadRequest(
                $"Portfolio com id {request.PortfolioId} não existe.");
        }

        var response = AssetMapper.ToDto(createdAsset);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _assetService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AssetDto>> Update(
    int id,
    UpdateAssetDto request)
    {
        var asset = AssetMapper.ToEntity(request);

        var result = await _assetService.UpdateAsync(id, asset);

        if (!result.PortfolioExists)
        {
            return BadRequest(
                $"Portfolio com id {request.PortfolioId} não existe.");
        }

        if (result.Asset is null)
        {
            return NotFound(
                $"Asset com id {id} não encontrado.");
        }

        return Ok(AssetMapper.ToDto(result.Asset));
    }
}