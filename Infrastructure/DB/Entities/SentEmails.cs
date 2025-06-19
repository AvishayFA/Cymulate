using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DB.Entities;

public record SentEmails
{
    [Key]
    public int Id { get; init; }

    [Required]
    public DateTime Timestamp { get; init; }

    [Required]
    [MaxLength(255)]
    public string ToEmailAddress { get; init; }

    public bool IsSuccess { get; init; }
}