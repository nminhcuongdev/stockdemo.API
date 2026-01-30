using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.DeliveryOrder
{
    public class BulkCreateDeliveryOrderDto
    {
        [Required(ErrorMessage = "ProductCode là bắt buộc")]
        [MaxLength(50, ErrorMessage = "ProductCode không được vượt quá 50 ký tự")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "PONumber là bắt buộc")]
        [MaxLength(100, ErrorMessage = "PONumber không được vượt quá 100 ký tự")]
        public string PONumber { get; set; }

        [Required(ErrorMessage = "DeliveryDate là bắt buộc")]
        public DateTime DeliveryDate { get; set; }

        [Required(ErrorMessage = "Quantity là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }
    }
}