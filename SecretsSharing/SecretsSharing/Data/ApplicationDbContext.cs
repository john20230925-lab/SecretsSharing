using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecretsSharing.Models;

namespace SecretsSharing.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FileSecret> FileSecrets { get; set; }
        public DbSet<TextSecret> TextSecrets { get; set; }
    }
}