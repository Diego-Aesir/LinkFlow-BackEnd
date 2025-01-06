namespace LinkFlowWebBFF.DTO
{
    public class CommentsToComments
    {
        public string? CommentParentId { get; set; }

        public required string OwnerId { get; set; }

        public DateTime? TimeStamp { get; set; } = DateTime.UtcNow;

        public required string Text { get; set; }

        public List<string>? CommentsId { get; set; } = new List<string>();
    }
}
