using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockOutRepository
{
    public class StockOutRepository : Repository<StockOut>, IStockOutRepository
    {
        public StockOutRepository(StockDemoDbContext context) : base(context) { }


        public async Task<StockOut> GetStockOutWithDetailsAsync(int stockOutId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StockOutId == stockOutId);
        }

        public async Task<IEnumerable<StockOut>> GetAllStockOutsWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockOut>> GetByProductAsync(int productId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .Where(s => s.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockOut>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
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
