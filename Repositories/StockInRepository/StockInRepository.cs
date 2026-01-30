using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockInRepository
{
    public class StockInRepository : Repository<StockIn>, IStockInRepository
    {
        public StockInRepository(StockDemoDbContext context) : base(context) { }

        public async Task<StockIn> GetStockInWithDetailsAsync(int stockInId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StockInId == stockInId);
        }

        public async Task<IEnumerable<StockIn>> GetAllStockInsWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockIn>> GetByProductAsync(int productId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .Where(s => s.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockIn>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .Where(s => s.CreatedDate >= startDate && s.CreatedDate <= endDate)
            .ToListAsync();
        }
    }
}
