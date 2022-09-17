namespace Whiteboard_API.Model
{

    public enum Role
    {
        Admin,
        User
    }


    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public Role role { get; set; } = Role.User; // user by default when account created

    }
}
