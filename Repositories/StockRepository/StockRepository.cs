using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockRepository
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(StockDemoDbContext context) : base(context) { }

        public async Task<Stock> GetByProductAndLocationAsync(int productId, int locationId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(s => s.ProductId == productId && s.LocationId == locationId);
        }

        public async Task<IEnumerable<Stock>> GetByProductAsync(int productId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Where(s => s.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stock>> GetByLocationAsync(int locationId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .Where(s => s.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<Stock> GetByQRCodeAsync(string qrCode)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(s => s.QRCode == qrCode);
        }

        public async Task<Stock> GetStockWithDetailsAsync(int stockId)
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(s => s.StockId == stockId);
        }

        public async Task<IEnumerable<Stock>> GetAllStocksWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Product)
                .Include(s => s.Location)
                .ToListAsync();
        }

        public async Task<bool> IncreaseQuantityAsync(int stockId, int amount)
        {
            var stock = await GetByIdAsync(stockId);
            if (stock == null)
                return false;

            stock.Quantity += amount;
            stock.LastUpdated = DateTime.Now;
            await UpdateAsync(stock);
            return true;
        }

        public async Task<bool> DecreaseQuantityAsync(int stockId, int amount)
        {
            var stock = await GetByIdAsync(stockId);
            if (stock == null)
                return false;

            // Keep the stock row (even at zero) so the product/location placement
            // is preserved for reporting and future stock-in.
            stock.Quantity -= amount;
            stock.LastUpdated = DateTime.Now;
            await UpdateAsync(stock);
            return true;
        }

        public async Task<bool> SetQuantityAsync(int stockId, int quantity)
        {
            var stock = await GetByIdAsync(stockId);
            if (stock == null)
                return false;

            stock.Quantity = quantity;
            stock.LastUpdated = DateTime.Now;
            await UpdateAsync(stock);
            return true;
        }
    }
}
