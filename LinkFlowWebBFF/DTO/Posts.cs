namespace LinkFlowWebBFF.DTO
{
    public class Posts
    {
        public string? Title { get; set; }

        public string? Photo { get; set; }

        public string? Text { get; set; }

        public List<string>? Tags { get; set; }

        public required string OwnerId { get; set; }
    }
}
