using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Stock
{
    public class UpdateStockDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải >= 0")]
        public int? Quantity { get; set; }

        [MaxLength(200)]
        public string QRCode { get; set; }
    }
}
