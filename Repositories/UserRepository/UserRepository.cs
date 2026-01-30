using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.BaseRepository;

namespace StockDemo.API.Repositories.UserRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(StockDemoDbContext context) : base(context) { }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _dbSet.Where(u => u.Role == role && u.IsActive).ToListAsync();
        }

        public async Task<bool> IsUsernameExistsAsync(string username, int? excludeId = null)
        {
            var query = _dbSet.Where(u => u.Username == username);
            if (excludeId.HasValue)
                query = query.Where(u => u.UserId != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                return false;

            user.LastLoginDate = DateTime.Now;
            await UpdateAsync(user);
            return true;
        }
    }
}
