using Cymulate2.Models.Entities;
using Infrastructure.DB.Entities;

namespace Cymulate2.Models.Interfaces;

public interface IPhishingService
{
    Task<List<SentEmails>> GetLastEmails(DateTime fromDate, DateTime toDate);

    Task<bool> SendPhishingEmailsAsync(PhishingRequest request);

    Task<int> GetEmailCount();
}
