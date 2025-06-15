namespace Cymulate2.Models.Entities;

public record UserInfo
{
    public string ID { get; init; }

    public string Email { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public DateTime CreatedAt { get; init; }
}
