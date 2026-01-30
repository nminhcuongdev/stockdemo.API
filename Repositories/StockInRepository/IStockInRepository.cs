using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockInRepository
{
    public interface IStockInRepository : IRepository<StockIn>
    {
        Task<StockIn> GetStockInWithDetailsAsync(int stockInId);
        Task<IEnumerable<StockIn>> GetAllStockInsWithDetailsAsync();
        Task<IEnumerable<StockIn>> GetByProductAsync(int productId);
        Task<IEnumerable<StockIn>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
