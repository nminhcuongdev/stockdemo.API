using StockDemo.API.Models.DTO.Product;

namespace StockDemo.API.Models.DTO.Report
{
    public class StockMovementReportDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int TotalStock { get; set; }
        public List<StockMovementReportItemDto> Items { get; set; } = new();
    }

    public class StockMovementReportItemDto
    {
        public int ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int CurrentStock { get; set; }
    }
}
