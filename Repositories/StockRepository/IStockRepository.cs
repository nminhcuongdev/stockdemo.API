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
        /// <summary>Adds <paramref name="amount"/> to the on-hand quantity (stock-in).</summary>
        Task<bool> IncreaseQuantityAsync(int stockId, int amount);

        /// <summary>Subtracts <paramref name="amount"/> from the on-hand quantity (stock-out). Caller must validate sufficiency.</summary>
        Task<bool> DecreaseQuantityAsync(int stockId, int amount);

        /// <summary>Sets the on-hand quantity to an absolute value (stocktake reconciliation).</summary>
        Task<bool> SetQuantityAsync(int stockId, int quantity);
    }
}
