using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.StockTransferRepository
{
    public interface IStockTransferRepository : IRepository<StockTransfer>
    {
        Task<StockTransfer> GetTransferWithDetailsAsync(int stockTransferId);
        Task<IEnumerable<StockTransfer>> GetAllTransfersWithDetailsAsync();
    }
}
