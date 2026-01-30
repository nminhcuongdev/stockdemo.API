using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.DeliveryOrder
{
    public class UpdateDeliveryOrderDto
    {
        public int? ProductId { get; set; }

        [MaxLength(100)]
        public string PONumber { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải > 0")]
        public int? Quantity { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
    }
}
