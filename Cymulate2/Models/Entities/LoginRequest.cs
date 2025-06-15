using System.ComponentModel.DataAnnotations;

namespace Cymulate2.Models.Entities;

public record LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    public string Password { get; init; }
}
