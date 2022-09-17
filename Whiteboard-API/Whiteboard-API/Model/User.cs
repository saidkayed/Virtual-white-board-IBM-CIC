namespace Whiteboard_API.Model
{

    public enum Role
    {
        Admin,
        User
    }

    public class ChangeUsernameDTO
    {

        public string Username { get; set; } = string.Empty;

    }

    public class UserDTO
    {
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
    class UserPostsDTO
    {
        public string Username { get; set; } = string.Empty;
        public List<Post> Posts { get; set; } = new List<Post>();
    }

    public class User
    {
        public int UserId { get; set; }

        public Role role { get; set; } = Role.User; // user by default when account created

        public string Username { get; set; } = String.Empty;

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public List<Post> posts { get; set; } = new List<Post>();
    }
}
