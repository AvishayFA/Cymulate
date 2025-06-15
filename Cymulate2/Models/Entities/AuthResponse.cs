namespace Cymulate2.Models.Entities;

public record AuthResponse
{
    public bool Success { get; init; }

    public string Message { get; init; }

    public string? Token { get; init; }

    public DateTime? ExpiresAt { get; init; }

    public UserInfo? User { get; init; }
}
