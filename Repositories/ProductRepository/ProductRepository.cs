using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.ProductRepository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(StockDemoDbContext context) : base(context) { }

        public async Task<Product> GetByCodeAsync(string productCode)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.ProductCode == productCode);
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _dbSet.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<bool> IsProductCodeExistsAsync(string productCode, int? excludeId = null)
        {
            var query = _dbSet.Where(p => p.ProductCode == productCode);
            if (excludeId.HasValue)
                query = query.Where(p => p.ProductId != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<Product> GetByProductCodeAsync(string productCode)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.ProductCode == productCode);
        }

        public async Task<List<Product>> GetByProductCodesAsync(List<string> productCodes)
        {
            return await _dbSet
                .Where(p => productCodes.Contains(p.ProductCode))
                .ToListAsync();
        }
    }
}
