using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Product
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        [MaxLength(50)]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [MaxLength(200)]
        public string ProductName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Đơn vị tính là bắt buộc")]
        [MaxLength(50)]
        public string Unit { get; set; }
    }
}
