using StockDemo.API.Models.Domain;

namespace StockDemo.API.Services
{
    public interface IJwtService
    {
        JwtTokenResult GenerateToken(User user);
    }
}