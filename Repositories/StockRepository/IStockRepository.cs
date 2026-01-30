using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockRepository
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock> GetByProductAndLocationAsync(int productId, int locationId);
        Task<IEnumerable<Stock>> GetByProductAsync(int productId);
        Task<IEnumerable<Stock>> GetByLocationAsync(int locationId);
        Task<Stock> GetByQRCodeAsync(string qrCode);
        Task<Stock> GetStockWithDetailsAsync(int stockId);
        Task<IEnumerable<Stock>> GetAllStocksWithDetailsAsync();
        Task<bool> UpdateQuantityAsync(int stockId, int quantity);
    }
}
