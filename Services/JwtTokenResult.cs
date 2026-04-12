namespace StockDemo.API.Services
{
    public class JwtTokenResult
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}