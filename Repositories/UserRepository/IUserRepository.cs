using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.UserRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<bool> IsUsernameExistsAsync(string username, int? excludeId = null);
        Task<bool> UpdateLastLoginAsync(int userId);
    }
}
