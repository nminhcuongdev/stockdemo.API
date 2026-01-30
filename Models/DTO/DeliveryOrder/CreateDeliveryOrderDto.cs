using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.DeliveryOrder
{
    public class CreateDeliveryOrderDto
    {
        [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Số PO là bắt buộc")]
        [MaxLength(100)]
        public string PONumber { get; set; }

        [Required(ErrorMessage = "Ngày giao hàng là bắt buộc")]
        public DateTime DeliveryDate { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải > 0")]
        public int Quantity { get; set; }
    }
}
