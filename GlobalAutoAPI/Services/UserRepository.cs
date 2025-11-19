using GlobalAutoLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalAutoAPI.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly GlobalAutoDBContext _context;

        public UserRepository(GlobalAutoDBContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.UserId == userId);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.OrderBy(u => u.Username).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}