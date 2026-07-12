using StockDemo.API.Models.DTO.Product;

namespace StockDemo.API.Models.DTO.Stock
{
    /// <summary>A product whose total on-hand quantity is below its reorder level (MinQuantity).</summary>
    public class LowStockItemDto
    {
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int CurrentQuantity { get; set; }
        public int MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public int Shortage { get; set; }
    }
}
