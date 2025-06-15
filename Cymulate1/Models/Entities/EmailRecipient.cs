using System.ComponentModel.DataAnnotations;

namespace Cymulate1.Models.Entities;

public record EmailRecipient
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }

    public IDictionary<string, string>? Parameters { get; init; }
}
