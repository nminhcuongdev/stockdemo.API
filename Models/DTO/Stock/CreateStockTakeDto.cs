using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Stock
{
    public class CreateStockTakeDto
    {
        [Required(ErrorMessage = "Vị trí kiểm kê là bắt buộc")]
        public int LocationId { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public int CreatedBy { get; set; }

        [Required(ErrorMessage = "Danh sách kiểm kê là bắt buộc")]
        [MinLength(1, ErrorMessage = "Phải có ít nhất một dòng kiểm kê")]
        public List<StockTakeCountLineDto> Items { get; set; } = new();
    }

    public class StockTakeCountLineDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng đếm phải >= 0")]
        public int CountedQuantity { get; set; }
    }
}
