using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UserAPI.DTO
{
    public class UpdateUser
    {
        [BsonElement("username")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        public string? UserName { get; set; }

        [BsonElement("name")]
        [Required(ErrorMessage = "name is required.")]
        public string? Name { get; set; }

        [BsonElement("email")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? UserEmail { get; set; }

        [BsonElement("pronoun")]
        public string? Pronoun { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("profile")]
        public string? Profile { get; set; }

        [BsonElement("gender")]
        public string? Gender { get; set; }

        [BsonElement("photo")]
        public string? Photo { get; set; }
    }
}
