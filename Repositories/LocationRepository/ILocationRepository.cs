using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.LocationRepository
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<Location> GetByCodeAsync(string locationCode);
        Task<IEnumerable<Location>> GetActiveLocationsAsync();
        Task<bool> IsLocationCodeExistsAsync(string locationCode, int? excludeId = null);
    }
}
