namespace Cymulate1.Models.Entities;

public record EmailSettings
{
    public string SmtpServer => "smtp.gmail.com";

    public int SmtpPort => 587;

    public string SenderEmail { get; init; }

    public string SenderPassword { get; init; }

    public string SenderName => "Email Simulation Server";
}