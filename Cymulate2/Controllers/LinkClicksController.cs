using Cymulate2.Models.Interfaces;
using Infrastructrure.Models.DB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cymulate2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LinkClicksController : ControllerBase
{
    private ILogger<LinkClicksController> _logger { get; }
    private ILinkClickService _linkClickService { get; }

    public LinkClicksController(ILinkClickService linkClickService, ILogger<LinkClicksController> logger)
    {
        _linkClickService = linkClickService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLinkClicks()
    {
        try
        {
            IEnumerable<LinkClick> linkClicks = await _linkClickService.GetAllAsync();

            return Ok(linkClicks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllLinkClicks endpoint");

            return StatusCode(500, "An error occurred while retrieving link clicks");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLinkClick(int id)
    {
        try
        {
            LinkClick linkClick = await _linkClickService.GetByIdAsync(id);

            if (linkClick == null)
            {
                return NotFound($"Link click with ID {id} not found");
            }

            return Ok(linkClick);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetLinkClick endpoint for ID {Id}", id);

            return StatusCode(500, "An error occurred while retrieving the link click");
        }
    }

    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> GetLinkClicksByEmail(string email)
    {
        try
        {
            IEnumerable<LinkClick> linkClicks = await _linkClickService.GetByEmailAsync(email);

            return Ok(linkClicks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetLinkClicksByEmail endpoint for email {Email}", email);

            return StatusCode(500, "An error occurred while retrieving link clicks");
        }
    }

    [HttpGet("by-date-range")]
    public async Task<IActionResult> GetLinkClicksByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            IEnumerable<LinkClick> linkClicks = await _linkClickService.GetByDateRangeAsync(startDate, endDate);

            return Ok(linkClicks);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetLinkClicksByDateRange endpoint");

            return StatusCode(500, "An error occurred while retrieving link clicks");
        }
    }
}
