using Cymulate1.Models.DB;
using Cymulate1.Models.DB.Interfaces;
using Infrastructrure.Models.DB.Entities;

namespace Cymulate1.Models.Services
{
    public class LinkTrackingService : ILinkTrackingService
    {
        private ILogger<LinkTrackingService> _logger { get; }
        private EmailTrackingContext _context { get; }

        public LinkTrackingService(EmailTrackingContext context, ILogger<LinkTrackingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RecordLinkClickAsync(string email)
        {
            var linkClick = new LinkClick
            {
                Timestamp = DateTime.UtcNow,
                Email = email
            };

            _context.LinkClicks.Add(linkClick);
            await _context.SaveChangesAsync();
        }
    }
}
