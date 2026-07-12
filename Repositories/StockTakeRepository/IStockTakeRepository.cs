using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockTakeRepository
{
    public interface IStockTakeRepository : IRepository<StockTake>
    {
        Task<StockTake> GetWithDetailsAsync(int stockTakeId);
        Task<IEnumerable<StockTake>> GetAllWithDetailsAsync();
    }
}
