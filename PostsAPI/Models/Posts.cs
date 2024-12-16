using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PostsAPI.Models
{
    public class Posts
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("posttitle")]
        public required string Title { get; set; }

        [BsonElement("photo")]
        public string? Photo { get; set; }

        [BsonElement("text")]
        public required string Text { get; set; }

        [BsonElement("tags")]
        public List<string>? Tags { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [BsonElement("owner")]
        public required string OwnerId { get; set; }
    }
}
