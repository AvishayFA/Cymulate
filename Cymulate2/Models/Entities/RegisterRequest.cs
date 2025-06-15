using System.ComponentModel.DataAnnotations;

namespace Cymulate2.Models.Entities;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    [MinLength(6)]
    public string Password { get; init; }

    [Required]
    public string FirstName { get; init; }

    [Required]
    public string LastName { get; init; }
}
