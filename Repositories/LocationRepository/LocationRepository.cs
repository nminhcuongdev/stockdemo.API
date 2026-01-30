using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.LocationRepository
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(StockDemoDbContext context) : base(context) { }

        public async Task<Location> GetByCodeAsync(string locationCode)
        {
            return await _dbSet.FirstOrDefaultAsync(l => l.LocationCode == locationCode);
        }

        public async Task<IEnumerable<Location>> GetActiveLocationsAsync()
        {
            return await _dbSet.Where(l => l.IsActive).ToListAsync();
        }

        public async Task<bool> IsLocationCodeExistsAsync(string locationCode, int? excludeId = null)
        {
            var query = _dbSet.Where(l => l.LocationCode == locationCode);
            if (excludeId.HasValue)
                query = query.Where(l => l.LocationId != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
