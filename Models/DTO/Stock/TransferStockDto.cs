using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Stock
{
    public class TransferStockDto
    {
        [Required(ErrorMessage = "Tồn kho nguồn là bắt buộc")]
        public int SourceStockId { get; set; }

        [Required(ErrorMessage = "Vị trí đích là bắt buộc")]
        public int ToLocationId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải > 0")]
        public int Quantity { get; set; }

        public int CreatedBy { get; set; }
    }
}
