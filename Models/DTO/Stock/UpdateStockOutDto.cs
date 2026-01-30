using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Stock
{
    public class UpdateStockOutDto
    {
        [MaxLength(50)]
        public string StockOutCode { get; set; }

        public int? ProductId { get; set; }

        public int? LocationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải > 0")]
        public int? Quantity { get; set; }

        [MaxLength(200)]
        public string QRCode { get; set; }

        [MaxLength(50)]
        public string Reason { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
    }
}
