namespace LinkFlowWebBFF.DTO
{
    public class Comments
    {
        public required string PostId { get; set; }

        public required string OwnerId { get; set; }

        public DateTime? TimeStamp { get; set; } = DateTime.UtcNow;

        public required string Text { get; set; }

        public List<string>? CommentsId { get; set; } = new List<string>();
    }
}
