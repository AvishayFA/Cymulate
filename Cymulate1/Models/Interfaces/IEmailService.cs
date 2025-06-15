using Cymulate1.Models.Entities;

namespace Cymulate1.Models.Interfaces;

public interface IEmailService
{
    Task<EmailResponse> SendEmailAsync(EmailRequest request);
}