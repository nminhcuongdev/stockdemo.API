using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;

namespace StockDemo.API.Repositories.EpcMappingRepository
{
    public class EpcMappingRepository : IEpcMappingRepository
    {
        private readonly StockDemoDbContext _context;

        public EpcMappingRepository(StockDemoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EpcMapping>> GetAllAsync()
        {
            return await _context.EpcMappings
                .Include(m => m.Stock).ThenInclude(s => s.Product)
                .Include(m => m.Stock).ThenInclude(s => s.Location)
                .ToListAsync();
        }

        public async Task<EpcMapping> GetByEpcAsync(string epc)
        {
            return await _context.EpcMappings
                .Include(m => m.Stock).ThenInclude(s => s.Product)
                .Include(m => m.Stock).ThenInclude(s => s.Location)
                .FirstOrDefaultAsync(m => m.Epc == epc);
        }

        public async Task<EpcMapping> AssignAsync(string epc, int stockId)
        {
            var existing = await _context.EpcMappings.FindAsync(epc);
            if (existing != null)
            {
                existing.StockId = stockId;
                existing.MappedDate = DateTime.Now;
            }
            else
            {
                existing = new EpcMapping { Epc = epc, StockId = stockId, MappedDate = DateTime.Now };
                _context.EpcMappings.Add(existing);
            }

            await _context.SaveChangesAsync();
            return await GetByEpcAsync(epc);
        }

        public async Task<bool> DeleteAsync(string epc)
        {
            var existing = await _context.EpcMappings.FindAsync(epc);
            if (existing == null)
                return false;

            _context.EpcMappings.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
