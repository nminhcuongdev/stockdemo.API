using StockDemo.API.Models.Domain;

namespace StockDemo.API.Repositories.EpcMappingRepository
{
    public interface IEpcMappingRepository
    {
        Task<IEnumerable<EpcMapping>> GetAllAsync();
        Task<EpcMapping> GetByEpcAsync(string epc);
        Task<EpcMapping> AssignAsync(string epc, int stockId);
        Task<bool> DeleteAsync(string epc);
    }
}
