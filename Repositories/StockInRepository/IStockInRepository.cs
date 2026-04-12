using StockDemo.API.Models.Domain;
using StockDemo.API.Models;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockInRepository
{
    public interface IStockInRepository : IRepository<StockIn>
    {
        Task<StockIn> GetStockInWithDetailsAsync(int stockInId);
        Task<PagedResult<StockIn>> GetAllStockInsWithDetailsAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            string? sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10);
        Task<IEnumerable<StockIn>> GetByProductAsync(int productId);
        Task<IEnumerable<StockIn>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}