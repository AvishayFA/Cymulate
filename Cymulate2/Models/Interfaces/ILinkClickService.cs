using Infrastructrure.Models.DB.Entities;

namespace Cymulate2.Models.Interfaces;

public interface ILinkClickService
{
    Task<IEnumerable<LinkClick>> GetAllAsync();

    Task<LinkClick?> GetByIdAsync(int id);

    Task<IEnumerable<LinkClick>> GetByEmailAsync(string email);

    Task<IEnumerable<LinkClick>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}