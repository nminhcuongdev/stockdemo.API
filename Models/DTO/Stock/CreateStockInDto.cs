using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Stock
{
    public class CreateStockInDto
    {
        [Required(ErrorMessage = "Sản phẩm là bắt buộc")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vị trí là bắt buộc")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải > 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "QR Code là bắt buộc")]
        [MaxLength(200)]
        public string QRCode { get; set; }
    }
}
