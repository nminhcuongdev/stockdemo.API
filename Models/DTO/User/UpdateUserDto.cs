using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.User
{
    public class UpdateUserDto
    {
        [MaxLength(200)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string Role { get; set; }

        public bool? IsActive { get; set; }
    }
}
