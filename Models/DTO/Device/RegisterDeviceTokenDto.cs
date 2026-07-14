using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.Device
{
    public class RegisterDeviceTokenDto
    {
        [Required(ErrorMessage = "Token là bắt buộc")]
        [MaxLength(500)]
        public string Token { get; set; }

        public int? UserId { get; set; }

        [MaxLength(20)]
        public string? Platform { get; set; }

        [MaxLength(10)]
        public string? Locale { get; set; }
    }
}
