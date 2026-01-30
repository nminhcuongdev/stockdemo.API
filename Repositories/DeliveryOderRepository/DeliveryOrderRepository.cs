using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.DeliveryOderRepository
{
    public class DeliveryOrderRepository : Repository<DeliveryOrder>, IDeliveryOrderRepository
    {
        public DeliveryOrderRepository(StockDemoDbContext context) : base(context) { }

        public async Task<DeliveryOrder> GetByPONumberAsync(string poNumber)
        {
            return await _dbSet
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d => d.PONumber == poNumber);
        }

        public async Task<DeliveryOrder> GetByQRCodeAsync(string qrCode)
        {
            return await _dbSet
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d => d.QRCode == qrCode);
        }

        public async Task<DeliveryOrder> GetDeliveryOrderWithDetailsAsync(int deliveryOrderId)
        {
            return await _dbSet
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d => d.DeliveryOrderId == deliveryOrderId);
        }

        public async Task<IEnumerable<DeliveryOrder>> GetAllDeliveryOrdersWithDetailsAsync()
        {
            return await _dbSet
                .Include(d => d.Product)
                .OrderByDescending(d => d.DeliveryDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DeliveryOrder>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(d => d.Product)
                .Where(d => d.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<DeliveryOrder>> GetByProductAsync(int productId)
        {
            return await _dbSet
                .Include(d => d.Product)
                .Where(d => d.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<DeliveryOrder>> GetByDeliveryDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(d => d.Product)
                .Where(d => d.DeliveryDate >= startDate && d.DeliveryDate <= endDate)
                .ToListAsync();
        }

        public async Task<bool> IsPONumberExistsAsync(string poNumber, int? excludeId = null)
        {
            var query = _dbSet.Where(d => d.PONumber == poNumber);
            if (excludeId.HasValue)
                query = query.Where(d => d.DeliveryOrderId != excludeId.Value);

            return await query.AnyAsync();
        }

        public string GenerateQRCode(string productCode, DateTime deliveryDate, string poNumber)
        {
            return $"{productCode};{deliveryDate:yyyy-MM-dd};{poNumber}";
        }

        // Kiểm tra trùng lặp theo cả 3 trường
        public async Task<List<DeliveryOrder>> GetDuplicateOrdersAsync(List<(int ProductId, string PONumber, DateTime DeliveryDate)> orders)
        {
            var duplicates = new List<DeliveryOrder>();

            foreach (var order in orders)
            {
                var existing = await _dbSet
                    .Include(d => d.Product)
                    .FirstOrDefaultAsync(d =>
                        d.ProductId == order.ProductId &&
                        d.PONumber == order.PONumber &&
                        d.DeliveryDate.Date == order.DeliveryDate.Date);

                if (existing != null)
                {
                    duplicates.Add(existing);
                }
            }

            return duplicates;
        }

        public async Task<List<DeliveryOrder>> AddRangeAsync(List<DeliveryOrder> deliveryOrders)
        {
            await _dbSet.AddRangeAsync(deliveryOrders);
            await _context.SaveChangesAsync();
            return deliveryOrders;
        }
    }
}