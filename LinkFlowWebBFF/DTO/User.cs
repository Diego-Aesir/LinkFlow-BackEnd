using System.ComponentModel.DataAnnotations;

namespace LinkFlowWebBFF.DTO
{
    public class User
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string UserEmail { get; set; }

        [Required(ErrorMessage = "Pronoun is required.")]
        public required string Pronoun { get; set; }

        public string? Password { get; set; }

        public string? Profile { get; set; }

        public required string Gender { get; set; }

        public string? Photo { get; set; }

        public bool? IsGoogle { get; set; }
    }
}
