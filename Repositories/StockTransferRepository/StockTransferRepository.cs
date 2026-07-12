using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockTransferRepository
{
    public class StockTransferRepository : Repository<StockTransfer>, IStockTransferRepository
    {
        public StockTransferRepository(StockDemoDbContext context) : base(context) { }

        public async Task<StockTransfer> GetTransferWithDetailsAsync(int stockTransferId)
        {
            return await _dbSet
                .Include(t => t.Product)
                .Include(t => t.FromLocation)
                .Include(t => t.ToLocation)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.StockTransferId == stockTransferId);
        }

        public async Task<IEnumerable<StockTransfer>> GetAllTransfersWithDetailsAsync()
        {
            return await _dbSet
                .Include(t => t.Product)
                .Include(t => t.FromLocation)
                .Include(t => t.ToLocation)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
        }
    }
}
