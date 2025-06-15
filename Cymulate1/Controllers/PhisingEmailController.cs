using Cymulate1.Models.Entities;
using Cymulate1.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cymulate1.Controllers;

[ApiController]
[Route("phishing")]
public class PhisingEmailController : Controller
{
    private ILogger<PhisingEmailController> _logger { get; }
    private IEmailService _emailService { get; }

    public PhisingEmailController(IEmailService emailService, ILogger<PhisingEmailController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<ActionResult<EmailResponse>> SendEmail([FromBody] EmailRequest request)
    {
        if (!request.IsValid())
        {
            return BadRequest("One or more email addresses are invalid");
        }

        try
        {
            var result = await _emailService.SendEmailAsync(request);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending personalized emails");

            return StatusCode(500, new EmailResponse
            {
                Success = false,
                Message = "Internal server error",
                SentAt = DateTime.UtcNow
            });
        }
    }

    [HttpGet("isAlive")]
    public ActionResult<string> Test()
    {
        return Ok("Phising server is running!");
    }
}