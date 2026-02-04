using FortivexAPI.Data;
using FortivexAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FortivexAPI.Services
{
    public class UserService
    {
        private readonly FortivexContext _context;

        public UserService(FortivexContext context)
        {
            _context = context;
        }

        // Csak az adatbázisból dolgozunk
        public async Task<User?> GetUserAsync(string username, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public User? GetUser(string username, string password)
        {
            // Fixed: use the context to query synchronously (or consider making this async)
            return _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
        }

        internal async Task<User> GetUserAsync(object userName, object password)
        {
            // Provide a basic implementation to avoid NotImplementedException
            var userNameStr = userName?.ToString() ?? string.Empty;
            var passwordStr = password?.ToString() ?? string.Empty;
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == userNameStr && u.PasswordHash == passwordStr);
        }

        // Changed return type from Task to Task<User?> and parameter type to string
        internal async Task<User?> GetUserByUsernameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
        }

        internal async Task CreateUserAsync(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }
    }
}
