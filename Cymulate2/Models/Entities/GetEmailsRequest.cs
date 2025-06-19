using System.ComponentModel.DataAnnotations;

namespace ManagementServer.Models.Entities;

public record GetEmailsRequest
{
    [Required]
    public DateTime FromDate { get; init; }

    [Required]
    public DateTime ToDate { get; init; }
}