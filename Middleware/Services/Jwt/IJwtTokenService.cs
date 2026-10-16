using System.Security.Claims;

namespace Middleware.Services.Jwt
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, string email);
        string GenerateResetToken(int userId, string email, DateTime expiry);
        Claim? ValidateToken(string token);
    }
}
