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

        [Range(0, int.MaxValue, ErrorMessage = "Định mức tối thiểu phải >= 0")]
        public int MinQuantity { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "Định mức tối đa phải >= 0")]
        public int? MaxQuantity { get; set; }
    }
}
