using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.ProductRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByCodeAsync(string productCode);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<bool> IsProductCodeExistsAsync(string productCode, int? excludeId = null);

        Task<Product> GetByProductCodeAsync(string productCode);
        Task<List<Product>> GetByProductCodesAsync(List<string> productCodes);
    }
}
