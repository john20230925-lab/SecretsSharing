namespace SecretsSharing.Models
{
    public class TextSecret
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Token { get; set; } // Secure random string for URL
        public bool AutoDelete { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}