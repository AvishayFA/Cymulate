namespace Cymulate1.Models.DB.Interfaces;

public interface ILinkTrackingService
{
    Task RecordLinkClickAsync(string email);
}
