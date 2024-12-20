using CommentsAPI.Models;

namespace CommentsAPI.DTO
{
    public class CommentToCommentReceived
    {
        public required string CommentParentId { get; set; }

        public required string OwnerId { get; set; }

        public required string Text { get; set; }

        public List<string> CommentsId { get; set; } = new List<string>();

        public CommentToComment TurnIntoModel()
        {
            CommentToComment commentFromComment = new()
            {
                CommentId = this.CommentParentId,
                OwnerId = this.OwnerId,
                Text = this.Text,
                CommentsId = this.CommentsId,
                TimeStamp = DateTime.UtcNow
            };

            return commentFromComment;
        }

    }
}
