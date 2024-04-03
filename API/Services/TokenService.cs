using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Config { get; }

        public string CreateToken(AppUser user)
        {
            // Create claims, key, and credentials
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? throw new ArgumentNullException("User name is missing")),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? throw new ArgumentNullException("User email is missing"))
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"] ?? throw new ArgumentNullException("TokenKey configuration is missing")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Create token descriptor
            var tokenDesciptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            // Create and return token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesciptior);
            return tokenHandler.WriteToken(token);
        }
    }

}