using Microsoft.Extensions.Configuration;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Middleware.Services.Jwt
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(int userId, string email)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(
                ClaimTypes.NameIdentifier, userId.ToString()));

            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim("TokenType", "Access"));

            return CreateToken(claims, DateTime.UtcNow.AddHours(1));
        }

        public string GenerateResetToken(
            int userId,
            string email,
            DateTime expiry)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(
                ClaimTypes.NameIdentifier, userId.ToString()));

            claims.Add(new Claim(ClaimTypes.Email, email));
            claims.Add(new Claim("TokenType", "PasswordReset"));

            return CreateToken(claims, expiry);
        }

        private string CreateToken(
            List<Claim> claims,
            DateTime expiry)
        {
            string? key = configuration["Jwt:Key"];
            string? issuer = configuration["Jwt:Issuer"];
            string? audience = configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(key) ||
                string.IsNullOrWhiteSpace(issuer) ||
                string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException(
                    "JWT configuration is incomplete.");
            }

            SymmetricSecurityKey securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            SigningCredentials credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiry,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Claim? ValidateToken(string token)
        {
            JwtSecurityTokenHandler handler =
                new JwtSecurityTokenHandler();

            string? key = configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            TokenValidationParameters parameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key)),
                    ClockSkew = TimeSpan.Zero
                };

            try
            {
                ClaimsPrincipal principal = handler.ValidateToken(
                    token,
                    parameters,
                    out SecurityToken validatedToken);

                return principal.FindFirst("TokenType");
            }
            catch (SecurityTokenException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}