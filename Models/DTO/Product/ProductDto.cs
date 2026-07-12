namespace StockDemo.API.Models.DTO.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
        public int MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
