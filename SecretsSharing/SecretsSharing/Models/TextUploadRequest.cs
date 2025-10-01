using System.ComponentModel.DataAnnotations;

namespace SecretsSharing.Models
{
    public class TextUploadRequest
    {
        [Required]
        public string Content { get; set; }
        public bool AutoDelete { get; set; }
    }
}