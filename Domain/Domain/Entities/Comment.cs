namespace Domain.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; } 
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
