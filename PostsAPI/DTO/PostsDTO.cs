using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PostsAPI.Models;

namespace PostsAPI.DTO
{
    public class PostsDTO
    {
        [BsonElement("posttitle")]
        public string? Title { get; set; }

        [BsonElement("photo")]
        public string? Photo { get; set; }

        [BsonElement("text")]
        public string? Text { get; set; }

        [BsonElement("tags")]
        public List<string>? Tags { get; set; }

        [BsonElement("owner")]
        public required string OwnerId { get; set; }

        public Posts ToPosts()
        {
            if (string.IsNullOrEmpty(this.OwnerId))
            {
                throw new ArgumentException("OwnerId is required");
            }

            return new Posts
            {
                Title = this.Title ?? "",
                Photo = this.Photo ?? "",  
                Text = this.Text ?? "",    
                Tags = this.Tags ?? new List<string>(),
                OwnerId = this.OwnerId ?? "",
                Date = DateTime.UtcNow
            };
        }
    }
}
