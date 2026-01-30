using StockDemo.API.Models.DTO.Location;
using StockDemo.API.Models.DTO.Product;
using StockDemo.API.Models.DTO.User;

namespace StockDemo.API.Models.DTO.Stock
{
    public class StockInDto
    {
        public int StockInId { get; set; }
        public string StockInCode { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int LocationId { get; set; }
        public LocationDto Location { get; set; }
        public int Quantity { get; set; }
        public string QRCode { get; set; }
        public int CreatedBy { get; set; }
        public UserDto User { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
