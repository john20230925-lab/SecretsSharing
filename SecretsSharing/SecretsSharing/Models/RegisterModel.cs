using System.ComponentModel.DataAnnotations;

namespace SecretsSharing.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}