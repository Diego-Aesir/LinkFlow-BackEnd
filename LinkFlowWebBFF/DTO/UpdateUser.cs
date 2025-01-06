using System.ComponentModel.DataAnnotations;

namespace LinkFlowWebBFF.DTO
{
    public class UpdateUser
    {
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        public string? UserName { get; set; }

        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? UserEmail { get; set; }

        public string? Pronoun { get; set; }

        public string? Password { get; set; }

        public string? Profile { get; set; }

        public string? Gender { get; set; }

        public string? Photo { get; set; }

        public bool? IsGoogle { get; set; }
    }
}
