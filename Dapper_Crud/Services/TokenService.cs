using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dapper_Crud.Services
{
    public class TokenService
    {
        private readonly string _key;
        private readonly ConcurrentDictionary<string, DateTime> _invalidatedTokens = new();

        public TokenService(IConfiguration config)
        {
            _key = config["Jwt:Key"]!;
        }

        public string GenerateToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User")
            };
            //new Claim("Department", user.Department)
            var token = new JwtSecurityToken(
                issuer: "srproskillbridge",
                audience: "srproskillbridge",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void InvalidateToken(string token)
        {
            _invalidatedTokens[token] = DateTime.UtcNow;
        }

        public bool IsTokenInvalidated(string token)
        {
            return _invalidatedTokens.ContainsKey(token);
        }
    }
}
