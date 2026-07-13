using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.DeviceTokenRepository
{
    public class DeviceTokenRepository : Repository<DeviceToken>, IDeviceTokenRepository
    {
        public DeviceTokenRepository(StockDemoDbContext context) : base(context) { }

        public async Task<DeviceToken> GetByTokenAsync(string token)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<List<string>> GetAllTokensAsync()
        {
            return await _dbSet.Select(t => t.Token).ToListAsync();
        }

        public async Task RemoveByTokensAsync(IEnumerable<string> tokens)
        {
            var toRemove = await _dbSet.Where(t => tokens.Contains(t.Token)).ToListAsync();
            if (toRemove.Count > 0)
            {
                _dbSet.RemoveRange(toRemove);
                await _context.SaveChangesAsync();
            }
        }
    }
}
