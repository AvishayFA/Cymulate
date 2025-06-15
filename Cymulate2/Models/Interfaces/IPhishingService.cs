using Cymulate2.Models.Entities;

namespace Cymulate2.Models.Interfaces;

public interface IPhishingService
{
    Task<bool> SendPhishingEmailsAsync(PhishingRequest request);
}
