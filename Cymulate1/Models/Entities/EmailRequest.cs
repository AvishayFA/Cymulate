using System.ComponentModel.DataAnnotations;

namespace Cymulate1.Models.Entities;

public record EmailRequest
{
    [Required]
    public List<EmailRecipient> Recipients { get; init; }

    [Required]
    public string Subject { get; init; }

    [Required]
    public string HtmlFileName { get; init; }

    public bool IsValid()
    {
        if (Recipients == null || Recipients.Count == 0)
            return false;

        return Recipients.All(recipient => !string.IsNullOrWhiteSpace(recipient.Email) && IsValidEmail(recipient.Email));
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);

            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}