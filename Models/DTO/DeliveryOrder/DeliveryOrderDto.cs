using StockDemo.API.Models.DTO.Product;

namespace StockDemo.API.Models.DTO.DeliveryOrder
{
    public class DeliveryOrderDto
    {
        public int DeliveryOrderId { get; set; }
        public int ProductId { get; set; }
        public string PONumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Quantity { get; set; }
        public string QRCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public ProductDto Product { get; set; }
    }
}
