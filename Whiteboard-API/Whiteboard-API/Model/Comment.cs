namespace Whiteboard_API.Model
{



    public class CommentDTO
    {
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }



    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public int PostId { get; set; } 
    }
}
