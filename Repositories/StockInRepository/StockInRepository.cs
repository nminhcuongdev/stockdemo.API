using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models;
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

        public async Task<PagedResult<StockIn>> GetAllStockInsWithDetailsAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var stockIns = _dbSet.Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .AsQueryable();

            // filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (filterOn.ToLower())
                {
                    case "qrcode":
                        stockIns = stockIns.Where(s => s.QRCode.Contains(filterQuery));
                        break;
                    case "quantity":
                        if (int.TryParse(filterQuery, out int quantity))
                        {
                            stockIns = stockIns.Where(s => s.Quantity == quantity);
                        }
                        break;
                    case "createddate":
                        if (DateTime.TryParse(filterQuery, out DateTime createdDate))
                        {
                            stockIns = stockIns.Where(s => s.CreatedDate.Date == createdDate.Date);
                        }
                        break;
                    case "productcode":
                        stockIns = stockIns.Where(s => s.Product.ProductCode.Contains(filterQuery));
                        break;
                    case "productname":
                        stockIns = stockIns.Where(s => s.Product.ProductName.Contains(filterQuery));
                        break;
                    case "locationname":
                        stockIns = stockIns.Where(s => s.Location.LocationName.Contains(filterQuery));
                        break;
                    case "username":
                        stockIns = stockIns.Where(s => s.User.Username.Contains(filterQuery));
                        break;
                        // add more cases as needed
                }
            }

            // sorting
            bool descending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "qrcode":
                        stockIns = descending ? stockIns.OrderByDescending(s => s.QRCode) : stockIns.OrderBy(s => s.QRCode);
                        break;
                    case "quantity":
                        stockIns = descending ? stockIns.OrderByDescending(s => s.Quantity) : stockIns.OrderBy(s => s.Quantity);
                        break;
                    case "createddate":
                        stockIns = descending ? stockIns.OrderByDescending(s => s.CreatedDate) : stockIns.OrderBy(s => s.CreatedDate);
                        break;
                    case "productcode":
                        stockIns = descending ? stockIns.OrderByDescending(s => s.Product.ProductCode) : stockIns.OrderBy(s => s.Product.ProductCode);
                        break;
                    case "productname":
                        stockIns = descending ? stockIns.OrderByDescending(s => s.Product.ProductName) : stockIns.OrderBy(s => s.Product.ProductName);
                        break;
                    case "locationname":
                        stockIns = descending ? stockIns.OrderByDescending(s => s.Location.LocationName) : stockIns.OrderBy(s => s.Location.LocationName);
                        break;
                    case "username":
                        stockIns = descending ? stockIns.OrderByDescending(s => s.User.Username) : stockIns.OrderBy(s => s.User.Username);
                        break;
                    default:
                        // unknown sort key -> fallback to CreatedDate desc
                        stockIns = stockIns.OrderByDescending(s => s.CreatedDate);
                        break;
                }
            }
            else
            {
                // default ordering: newest first
                stockIns = stockIns.OrderByDescending(s => s.CreatedDate);
            }

            // pagination: sanitize inputs
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Max(1, Math.Min(pageSize, 100)); // cap pageSize to 100

            var totalCount = await stockIns.CountAsync();

            var items = await stockIns
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<StockIn>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
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