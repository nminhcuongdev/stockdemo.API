using StockDemo.API.Models.DTO.Location;
using StockDemo.API.Models.DTO.Product;
using StockDemo.API.Models.DTO.User;

namespace StockDemo.API.Models.DTO.Stock
{
    public class StockTakeDto
    {
        public int StockTakeId { get; set; }
        public int LocationId { get; set; }
        public LocationDto Location { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public int CreatedBy { get; set; }
        public UserDto User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public List<StockTakeItemDto> Items { get; set; } = new();
    }

    public class StockTakeItemDto
    {
        public int StockTakeItemId { get; set; }
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int SystemQuantity { get; set; }
        public int CountedQuantity { get; set; }
        public int Variance { get; set; }
    }
}
