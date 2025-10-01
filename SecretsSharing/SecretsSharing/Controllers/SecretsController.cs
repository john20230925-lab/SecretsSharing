using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecretsSharing.Data;
using SecretsSharing.Models;
using SecretsSharing.Services;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SecretsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // POST: api/secrets/file
        [Authorize]
        [HttpPost("file")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            var token = TokenGenerator.GenerateToken();
            var uploadsFolder = Path.Combine(_env.ContentRootPath, "Uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var fileSecret = new FileSecret
            {
                Id = Guid.NewGuid(),
                FilePath = filePath,
                Token = token,
                AutoDelete = request.AutoDelete,
                CreatedAt = DateTime.UtcNow,
                OriginalFileName = request.File.FileName
            };

            _db.FileSecrets.Add(fileSecret);
            await _db.SaveChangesAsync();

            var url = Url.Action(nameof(DownloadFile), "Secrets", new { token = token }, Request.Scheme);
            return Ok(new { url });
        }

        // GET: api/secrets/file/{token}
        [AllowAnonymous]
        [HttpGet("file/{token}")]
        public async Task<IActionResult> DownloadFile(string token)
        {
            var fileSecret = await _db.FileSecrets.FirstOrDefaultAsync(f => f.Token == token);
            if (fileSecret == null || !System.IO.File.Exists(fileSecret.FilePath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(fileSecret.FilePath);
            var fileName = fileSecret.OriginalFileName ?? "downloaded_file";

            if (fileSecret.AutoDelete)
            {
                System.IO.File.Delete(fileSecret.FilePath);
                _db.FileSecrets.Remove(fileSecret);
                await _db.SaveChangesAsync();
            }

            return File(fileBytes, "application/octet-stream", fileName);
        }

        // POST: api/secrets/text
        [Authorize]
        [HttpPost("text")]
        public async Task<IActionResult> UploadText([FromBody] TextUploadRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Content is required.");

            var token = TokenGenerator.GenerateToken();

            var textSecret = new TextSecret
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                Token = token,
                AutoDelete = request.AutoDelete,
                CreatedAt = DateTime.UtcNow
            };

            _db.TextSecrets.Add(textSecret);
            await _db.SaveChangesAsync();

            var url = Url.Action(nameof(GetText), "Secrets", new { token = token }, Request.Scheme);
            return Ok(new { url });
        }

        // GET: api/secrets/text/{token}
        [AllowAnonymous]
        [HttpGet("text/{token}")]
        public async Task<IActionResult> GetText(string token)
        {
            var textSecret = await _db.TextSecrets.FirstOrDefaultAsync(t => t.Token == token);
            if (textSecret == null)
                return NotFound();

            var content = textSecret.Content;

            if (textSecret.AutoDelete)
            {
                _db.TextSecrets.Remove(textSecret);
                await _db.SaveChangesAsync();
            }

            return Ok(new { content });
        }
    }
}