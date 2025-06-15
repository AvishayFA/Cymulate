using System.Security.Claims;
using Cymulate2.Models.Entities;

namespace Cymulate2.Models.Interfaces;

public interface IJwtService
{
    ClaimsPrincipal? ValidateToken(string token);

    string GenerateToken(ApplicationUser user);
}
