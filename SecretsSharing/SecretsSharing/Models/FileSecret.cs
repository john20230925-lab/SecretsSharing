namespace SecretsSharing.Models
{
    public class FileSecret
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string Token { get; set; } // Secure random string for URL
        public bool AutoDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? OriginalFileName { get; set; }
    }
}