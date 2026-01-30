namespace StockDemo.API.Models.DTO.User
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
