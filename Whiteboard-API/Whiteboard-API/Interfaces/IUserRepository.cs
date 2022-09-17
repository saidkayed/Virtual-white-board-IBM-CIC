using Whiteboard_API.Model;

namespace Whiteboard_API.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<User> DeleteUser(int id);

        Task<User> GetUserByUsername(string username);

        Task<bool> DoUsernameExist(string username);
        
    }
}
