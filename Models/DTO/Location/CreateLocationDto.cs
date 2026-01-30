using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Location
{
    public class CreateLocationDto
    {
        [Required(ErrorMessage = "Mã vị trí là bắt buộc")]
        [MaxLength(50)]
        public string LocationCode { get; set; }

        [Required(ErrorMessage = "Tên vị trí là bắt buộc")]
        [MaxLength(200)]
        public string LocationName { get; set; }
    }
}
