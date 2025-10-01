using System.ComponentModel.DataAnnotations;

namespace SecretsSharing.Models
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
        public bool AutoDelete { get; set; }
    }
}