using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.DeliveryOderRepository
{
    public interface IDeliveryOrderRepository : IRepository<DeliveryOrder>
    {
        Task<DeliveryOrder> GetByPONumberAsync(string poNumber);
        Task<DeliveryOrder> GetByQRCodeAsync(string qrCode);
        Task<DeliveryOrder> GetDeliveryOrderWithDetailsAsync(int deliveryOrderId);
        Task<IEnumerable<DeliveryOrder>> GetAllDeliveryOrdersWithDetailsAsync();
        Task<IEnumerable<DeliveryOrder>> GetByStatusAsync(string status);
        Task<IEnumerable<DeliveryOrder>> GetByProductAsync(int productId);
        Task<IEnumerable<DeliveryOrder>> GetByDeliveryDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<bool> IsPONumberExistsAsync(string poNumber, int? excludeId = null);
        string GenerateQRCode(string productCode, DateTime deliveryDate, string poNumber);

        // Thay đổi method kiểm tra trùng lặp
        Task<List<DeliveryOrder>> GetDuplicateOrdersAsync(List<(int ProductId, string PONumber, DateTime DeliveryDate)> orders);
        Task<List<DeliveryOrder>> AddRangeAsync(List<DeliveryOrder> deliveryOrders);
    }
}