using System.ComponentModel.DataAnnotations;

namespace Cymulate2.Models.Entities;

public record PhishingRequest
{
    [Required]
    public List<Recipient> Recipients { get; init; }

    [Required]
    public string Subject { get; init; }

    [Required]
    public string HtmlFileName { get; init; }
}
