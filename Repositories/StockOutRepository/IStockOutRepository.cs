using StockDemo.API.Models.Domain;
using StockDemo.API.Models;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockOutRepository
{
    public interface IStockOutRepository : IRepository<StockOut>
    {
        Task<StockOut> GetStockOutWithDetailsAsync(int stockOutId);
        Task<PagedResult<StockOut>> GetAllStockOutsWithDetailsAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10);
        Task<IEnumerable<StockOut>> GetByProductAsync(int productId);
        Task<IEnumerable<StockOut>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
