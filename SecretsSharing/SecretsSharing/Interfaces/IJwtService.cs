using System.Security.Claims;

namespace SecretsSharing.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}