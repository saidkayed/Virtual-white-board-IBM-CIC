namespace Whiteboard_API.Model
{

    public class PostDTO
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int UserId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }


    public class Post
    {
        public int PostId { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool isAnonymous { get; set; } = false;

        public string Content { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        public int Likes { get; set; }
        
        public List<Comment> comments { get; set; } = new List<Comment>();

        public int UserId { get; set; } = default!;

    }
}
