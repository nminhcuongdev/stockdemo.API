using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockOutRepository
{
    public interface IStockOutRepository : IRepository<StockOut>
    {
        Task<StockOut> GetStockOutWithDetailsAsync(int stockOutId);
        Task<IEnumerable<StockOut>> GetAllStockOutsWithDetailsAsync();
        Task<IEnumerable<StockOut>> GetByProductAsync(int productId);
        Task<IEnumerable<StockOut>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
