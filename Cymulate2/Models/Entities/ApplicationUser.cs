using Microsoft.AspNetCore.Identity;

namespace Cymulate2.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; init; }

    public string LastName { get; init; }

    public DateTime CreatedAt { get; init; }
}
