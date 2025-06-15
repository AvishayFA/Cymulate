using Cymulate1.Models.DB.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cymulate1.Controllers;

[ApiController]
[Route("tracking")]
public class LinkTrackingController : ControllerBase
{
    private ILinkTrackingService _linkTrackingService { get; }
    private ILogger<LinkTrackingController> _logger { get; }

    public LinkTrackingController(ILinkTrackingService linkTrackingService, ILogger<LinkTrackingController> logger)
    {
        _linkTrackingService = linkTrackingService;
        _logger = logger;
    }

    [HttpGet("click")]
    public async Task<IActionResult> TrackClick([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email parameter is required");
        }

        try
        {
            await _linkTrackingService.RecordLinkClickAsync(email);

            return Ok(new
            {
                message = "Link click recorded successfully",
                timestamp = DateTime.UtcNow,
                email = email
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording link click");

            return StatusCode(500, "Internal Server Error");
        }
    }
}