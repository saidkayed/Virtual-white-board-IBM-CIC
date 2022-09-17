using Microsoft.EntityFrameworkCore;
using Whiteboard_API.Data;
using Whiteboard_API.Interfaces;
using Whiteboard_API.Model;

namespace Whiteboard_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Whiteboard_APIContext _context;
        public UserRepository(Whiteboard_APIContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.User.ToListAsync();
        }
        public async Task<User> GetUserById(int id)
        {
            return await _context.User.FindAsync(id);
        }
        public async Task<User> CreateUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<User> GetUserByUsername(string username)
        {
            return _context.User.FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<bool> DoUsernameExist(string username)
        {
            return _context.User.AnyAsync(u => u.Username == username);
        }
    }
}
