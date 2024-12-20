using MongoDB.Bson.Serialization.Attributes;

namespace CommentsAPI.Models
{
    public class CommentToComment
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("response_from_comment_id")]
        public required string CommentId { get; set; }

        [BsonElement("owner_id")]
        public required string OwnerId { get; set; }

        [BsonElement("timestamp")]
        public required DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        [BsonElement("text")]
        public required string Text { get; set; }

        [BsonElement("comments")]
        public List<string> CommentsId { get; set; } = new List<string>();
    }
}
