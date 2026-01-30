using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Stock
{
    public class CreateStockDto
    {
        [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vị trí là bắt buộc")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "QR Code là bắt buộc")]
        [MaxLength(200)]
        public string QRCode { get; set; }
        public int UserId { get; set; }
    }
}
