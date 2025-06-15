using Cymulate2.Models.Entities;
using Cymulate2.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cymulate2.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PhishingController : ControllerBase
{
    private ILogger<PhishingController> _logger { get; }
    private IPhishingService _phishingService { get; }

    public PhishingController(IPhishingService phishingService, ILogger<PhishingController> logger)
    {
        _phishingService = phishingService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendPhishingEmails([FromBody] PhishingRequest request)
    {
        try
        {
            if (!request.Recipients.Any())
            {
                return BadRequest("At least one recipient is required");
            }

            bool success = await _phishingService.SendPhishingEmailsAsync(request);

            if (success)
            {
                return Ok(new { message = "Phishing emails sent successfully", recipientCount = request.Recipients.Count });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to send some or all phishing emails" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendPhishingEmails endpoint");

            return StatusCode(500, new { message = "An error occurred while sending phishing emails" });
        }
    }
}