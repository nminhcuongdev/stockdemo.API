using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Product
{
    public class UpdateProductDto
    {
        [MaxLength(50)]
        public string ProductCode { get; set; }

        [MaxLength(200)]
        public string ProductName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string Unit { get; set; }

        public bool? IsActive { get; set; }
    }
}
