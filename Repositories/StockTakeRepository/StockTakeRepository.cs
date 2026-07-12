using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockTakeRepository
{
    public class StockTakeRepository : Repository<StockTake>, IStockTakeRepository
    {
        public StockTakeRepository(StockDemoDbContext context) : base(context) { }

        public async Task<StockTake> GetWithDetailsAsync(int stockTakeId)
        {
            return await _dbSet
                .Include(s => s.Location)
                .Include(s => s.User)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.StockTakeId == stockTakeId);
        }

        public async Task<IEnumerable<StockTake>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Location)
                .Include(s => s.User)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Product)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
        }
    }
}
