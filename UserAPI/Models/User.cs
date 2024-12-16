using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("username")]
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        public required string UserName { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "name is required.")]
        public required string Name { get; set; }

        [BsonElement("email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string UserEmail { get; set; }

        [BsonElement("pronoun")]
        [Required(ErrorMessage = "Pronoun is required.")]
        public required string Pronoun { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("profile")]
        public string? Profile { get; set; }

        [BsonElement("gender")]
        public required string Gender { get; set; }

        [BsonElement("photo")]
        public string? Photo { get; set; }    
    }
}
