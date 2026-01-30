using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vai trò là bắt buộc")]
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
