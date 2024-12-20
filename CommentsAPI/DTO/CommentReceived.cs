using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CommentsAPI.Model;

namespace CommentsAPI.DTO
{
    public class CommentReceived
    {
        [BsonElement("post_id")]
        public required string PostId { get; set; }

        [BsonElement("owner_id")]
        public required string OwnerId { get; set; }

        [BsonElement("text")]
        public required string Text { get; set; }

        [BsonElement("comments")]
        public List<string> CommentsId { get; set; } = new List<string>();

        public Comment createCommentModel()
        {
            Comment comment = new()
            { 
                OwnerId = this.OwnerId,
                PostId = this.PostId,
                Text = this.Text,
                TimeStamp = DateTime.UtcNow,
                CommentsId = this.CommentsId
            };

            return comment;
        }
    }
}
