using System.ComponentModel.DataAnnotations;

namespace Cymulate2.Models.Entities;

public record Recipient
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }

    public Dictionary<string, string> Parameters { get; init; }
}