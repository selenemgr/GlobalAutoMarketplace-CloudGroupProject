using GlobalAutoLibrary.Models;

namespace GlobalAutoAPI.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        void DeleteUser(User user);
        Task<bool> UserExistsAsync(int userId);
        Task<bool> SaveAsync();
    }
}