using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.DeviceTokenRepository
{
    public interface IDeviceTokenRepository : IRepository<DeviceToken>
    {
        Task<DeviceToken> GetByTokenAsync(string token);
        Task<List<string>> GetAllTokensAsync();
        Task RemoveByTokensAsync(IEnumerable<string> tokens);
    }
}
