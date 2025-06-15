using System.ComponentModel.DataAnnotations;

namespace Infrastructrure.Models.DB.Entities;

public record LinkClick
{
    [Key]
    public int Id { get; init; }

    [Required]
    public DateTime Timestamp { get; init; }

    [Required]
    [MaxLength(255)]
    public string Email { get; init; }
}
