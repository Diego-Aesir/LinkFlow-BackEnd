using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CommentsAPI.Model
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("post_id")]
        public required string PostId {get; set; }

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
