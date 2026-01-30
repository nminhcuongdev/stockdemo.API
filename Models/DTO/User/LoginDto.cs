using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.User
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; }
    }
}
