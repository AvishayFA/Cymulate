using System.Net;
using System.Net.Mail;
using Cymulate2.Models.Entities;
using Cymulate2.Models.Interfaces;

namespace Cymulate2.Models.Services;

public class PhishingService : IPhishingService
{
    private readonly ILogger<PhishingService> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    public PhishingService(ILogger<PhishingService> logger, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
    }

    public async Task<bool> SendPhishingEmailsAsync(PhishingRequest request)
    {
        var htmlContent = await LoadHtmlTemplateAsync(request.HtmlFileName);

        if (string.IsNullOrEmpty(htmlContent))
        {
            return false;
        }

        var tasks = request.Recipients.Select(recipient => SendEmailToRecipientAsync(recipient, request.Subject, htmlContent));
        var results = await Task.WhenAll(tasks);
        var successCount = results.Count(r => r);

        return successCount == request.Recipients.Count;
    }

    private async Task<string> LoadHtmlTemplateAsync(string fileName)
    {
        var templatePath = Path.Combine(_webHostEnvironment.WebRootPath ?? _webHostEnvironment.ContentRootPath, "templates", fileName);

        if (!File.Exists(templatePath))
        {
            return string.Empty;
        }

        return await File.ReadAllTextAsync(templatePath);
    }

    private async Task<bool> SendEmailToRecipientAsync(Recipient recipient, string subject, string htmlTemplate)
    {
        var personalizedHtml = ReplaceParameters(htmlTemplate, recipient.Parameters);

        var smtpHost = _configuration["SmtpSettings:Host"];
        var smtpPort = int.Parse(_configuration["SmtpSettings:Port"] ?? "587");
        var smtpUsername = _configuration["SmtpSettings:Username"];
        var smtpPassword = _configuration["SmtpSettings:Password"];
        var fromEmail = _configuration["SmtpSettings:FromEmail"];
        var fromName = _configuration["SmtpSettings:FromName"] ?? "Phishing Simulation";
        var enableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"] ?? "true");

        using var smtpClient = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = enableSsl
        };

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = personalizedHtml,
            IsBodyHtml = true
        };

        mailMessage.To.Add(recipient.Email);
        await smtpClient.SendMailAsync(mailMessage);

        return true;
    }

    private string ReplaceParameters(string htmlTemplate, Dictionary<string, string> parameters)
    {
        var result = htmlTemplate;

        foreach (var param in parameters)
        {
            string placeholder = $"{{{{{param.Key}}}}}";
            result = result.Replace(placeholder, param.Value);
        }

        return result;
    }
}
