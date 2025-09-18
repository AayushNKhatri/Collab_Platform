using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class JWTtokenService : ITokenService
    {
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _config;
        public JWTtokenService(IUserRepo userRepo, IConfiguration config) {
            _userRepo = userRepo;
            _config = config;
        }
        public async Task<string> GenerateJwtToken(UserModel user)
        {
            try {
                var roles = await _userRepo.GetUserRole(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                if (!string.IsNullOrEmpty(roles))
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles));
                }

                else
                {
                    claims.Add(new(ClaimTypes.Role, "User"));
                }
                var tokenKey = _config["TokenSettings:Token"];
                if (string.IsNullOrEmpty(tokenKey)) throw new InvalidOperationException("Token Key is Missing in Configuration");
                var issuer = _config["TokenSettings:Issuer"];
                var audience = _config["TokenSettings:Audience"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
                if (string.IsNullOrEmpty(tokenKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {
          
                    throw new InvalidOperationException("JWT environment variables not configured properly.");
                }
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                var descriptor = new JwtSecurityToken(
                        issuer:issuer,
                        audience:audience,
                        expires:DateTime.UtcNow.AddDays(1),
                        signingCredentials:credentials,
                        claims:claims
                    );

                return new JwtSecurityTokenHandler().WriteToken(descriptor);
          

            }
            catch(Exception e) 
            {
                throw new Exception("Unable to create token" + e.Message);
            }
        }
    }
}
