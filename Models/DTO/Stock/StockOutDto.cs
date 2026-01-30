using StockDemo.API.Models.DTO.Location;
using StockDemo.API.Models.DTO.Product;
using StockDemo.API.Models.DTO.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockDemo.API.Models.DTO.Stock
{
    public class StockOutDto
    {
        public int StockOutId { get; set; }
        public string StockOutCode { get; set; }
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public int Quantity { get; set; }
        public string QRCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductDto Product { get; set; }
        public LocationDto Location { get; set; }

        public UserDto User { get; set; }
    }
}
