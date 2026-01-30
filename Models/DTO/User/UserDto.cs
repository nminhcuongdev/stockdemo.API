using StockDemo.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace StockDemo.API.Models.DTO.User
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
