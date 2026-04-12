using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Models;
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

        public async Task<PagedResult<StockOut>> GetAllStockOutsWithDetailsAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10)
        {
            var stockOuts = _dbSet.Include(s => s.Product)
                .Include(s => s.Location)
                .Include(s => s.User)
                .AsQueryable();

            // filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                switch (filterOn.ToLower())
                {
                    case "qrcode":
                        stockOuts = stockOuts.Where(s => s.QRCode.Contains(filterQuery));
                        break;
                    case "quantity":
                        if (int.TryParse(filterQuery, out int quantity))
                        {
                            stockOuts = stockOuts.Where(s => s.Quantity == quantity);
                        }
                        break;
                    case "createddate":
                        if (DateTime.TryParse(filterQuery, out DateTime createdDate))
                        {
                            stockOuts = stockOuts.Where(s => s.CreatedDate.Date == createdDate.Date);
                        }
                        break;
                    case "productcode":
                        stockOuts = stockOuts.Where(s => s.Product.ProductCode.Contains(filterQuery));
                        break;
                    case "productname":
                        stockOuts = stockOuts.Where(s => s.Product.ProductName.Contains(filterQuery));
                        break;
                    case "locationname":
                        stockOuts = stockOuts.Where(s => s.Location.LocationName.Contains(filterQuery));
                        break;
                    case "username":
                        stockOuts = stockOuts.Where(s => s.User.Username.Contains(filterQuery));
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
                        stockOuts = descending ? stockOuts.OrderByDescending(s => s.QRCode) : stockOuts.OrderBy(s => s.QRCode);
                        break;
                    case "quantity":
                        stockOuts = descending ? stockOuts.OrderByDescending(s => s.Quantity) : stockOuts.OrderBy(s => s.Quantity);
                        break;
                    case "createddate":
                        stockOuts = descending ? stockOuts.OrderByDescending(s => s.CreatedDate) : stockOuts.OrderBy(s => s.CreatedDate);
                        break;
                    case "productcode":
                        stockOuts = descending ? stockOuts.OrderByDescending(s => s.Product.ProductCode) : stockOuts.OrderBy(s => s.Product.ProductCode);
                        break;
                    case "productname":
                        stockOuts = descending ? stockOuts.OrderByDescending(s => s.Product.ProductName) : stockOuts.OrderBy(s => s.Product.ProductName);
                        break;
                    case "locationname":
                        stockOuts = descending ? stockOuts.OrderByDescending(s => s.Location.LocationName) : stockOuts.OrderBy(s => s.Location.LocationName);
                        break;
                    case "username":
                        stockOuts = descending ? stockOuts.OrderByDescending(s => s.User.Username) : stockOuts.OrderBy(s => s.User.Username);
                        break;
                    default:
                        // unknown sort key -> fallback to CreatedDate desc
                        stockOuts = stockOuts.OrderByDescending(s => s.CreatedDate);
                        break;
                }
            }
            else
            {
                // default ordering: newest first
                stockOuts = stockOuts.OrderByDescending(s => s.CreatedDate);
            }

            // pagination: sanitize inputs
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Max(1, Math.Min(pageSize, 100)); // cap pageSize to 100

            var totalCount = await stockOuts.CountAsync();

            var items = await stockOuts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<StockOut>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
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
