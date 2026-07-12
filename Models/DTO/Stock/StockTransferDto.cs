using StockDemo.API.Models.DTO.Location;
using StockDemo.API.Models.DTO.Product;
using StockDemo.API.Models.DTO.User;

namespace StockDemo.API.Models.DTO.Stock
{
    public class StockTransferDto
    {
        public int StockTransferId { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int FromLocationId { get; set; }
        public LocationDto FromLocation { get; set; }
        public int ToLocationId { get; set; }
        public LocationDto ToLocation { get; set; }
        public int Quantity { get; set; }
        public string QRCode { get; set; }
        public int CreatedBy { get; set; }
        public UserDto User { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
