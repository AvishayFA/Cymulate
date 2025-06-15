namespace Cymulate1.Models.Entities;

public record EmailResponse
{
    public bool Success { get; init; }

    public string Message { get; init; }

    public DateTime SentAt { get; init; }

    public int EmailsSent { get; init; }

    public List<string> FailedEmails { get; init; }
}
