using System.Net;
using System.Net.Mail;
using Cymulate1.Models.DB;
using Cymulate1.Models.Entities;
using Cymulate1.Models.Interfaces;
using Infrastructure.DB.Entities;
using Microsoft.Extensions.Options;

namespace Cymulate1.Models.Services;

public class EmailService : IEmailService
{
    private ILogger<EmailService> _logger { get; }
    private EmailTrackingContext _context { get; }
    private EmailSettings _emailSettings { get; }

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, EmailTrackingContext context)
    {
        _emailSettings = emailSettings.Value;
        _context = context;
        _logger = logger;
    }

    public async Task<EmailResponse> SendEmailAsync(EmailRequest request)
    {
        var response = new EmailResponse
        {
            SentAt = DateTime.UtcNow,
            FailedEmails = [],
            Success = false,
            EmailsSent = 0
        };

        using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);

        foreach (var recipient in request.Recipients)
        {
            try
            {
                string personalizedBody = await LoadHtmlFileAsync(request.HtmlFileName, recipient.Parameters);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = request.Subject,
                    Body = personalizedBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipient.Email);
                await client.SendMailAsync(mailMessage);
                _context.SentEmails.Add(new SentEmails
                {
                    ToEmailAddress = recipient.Email,
                    Timestamp = DateTime.UtcNow,
                    IsSuccess = true
                });
            }
            catch (Exception emailEx)
            {
                response.FailedEmails.Add(recipient.Email);
                _logger.LogError(emailEx, $"Failed to send email to {recipient.Email}");
                _context.SentEmails.Add(new SentEmails
                {
                    ToEmailAddress = recipient.Email,
                    Timestamp = DateTime.UtcNow,
                    IsSuccess = false
                });
            }
        }

        return response;
    }

    private async Task<string> LoadHtmlFileAsync(string fileName, IDictionary<string, string> parameters)
    {
        string filePath = Path.Combine("Emails", fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"HTML file '{fileName}' not found at {filePath}");
        }

        string htmlContent = await File.ReadAllTextAsync(filePath);

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                htmlContent = htmlContent.Replace($"{{{{{param.Key}}}}}", param.Value);
            }
        }

        return htmlContent;
    }
}