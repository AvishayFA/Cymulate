using Cymulate2.Models.DB;
using Cymulate2.Models.Interfaces;
using Infrastructrure.Models.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cymulate2.Models.Services
{
    public class LinkClickService : ILinkClickService
    {
        private UsersDbContext _context { get; }
        private ILogger<LinkClickService> _logger { get; }

        public LinkClickService(UsersDbContext context, ILogger<LinkClickService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<LinkClick>> GetAllAsync()
        {
            return await _context.LinkClicks
                .OrderByDescending(lc => lc.Timestamp)
                .ToListAsync();
        }

        public async Task<LinkClick?> GetByIdAsync(int id)
        {
            return await _context.LinkClicks.FindAsync(id);
        }

        public async Task<IEnumerable<LinkClick>> GetByEmailAsync(string email)
        {
            return await _context.LinkClicks
                     .Where(lc => lc.Email.ToLower() == email.ToLower())
                     .OrderByDescending(lc => lc.Timestamp)
                     .ToListAsync();
        }

        public async Task<IEnumerable<LinkClick>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date");
            }

            return await _context.LinkClicks
                .Where(lc => lc.Timestamp >= startDate && lc.Timestamp <= endDate)
                .OrderByDescending(lc => lc.Timestamp)
                .ToListAsync();
        }
    }
}