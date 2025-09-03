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
                var claim = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.UserName)
                };
                var roles = await _userRepo.GetUserRole(user);
                if (roles.Any())
                {
                    foreach (var role in roles)
                    {
                        claim.Add(new(ClaimTypes.Role, roles));
                    }
                }
                else {
                    claim.Add(new(ClaimTypes.Role, "User"));
                }
                var tokenKey = _config["TokenSetting:Token"];
                if (string.IsNullOrEmpty(tokenKey)) throw new InvalidOperationException("Token Key is Missing in Configuration");
                var issuer = _config["TokenSetting:Issuer"];
                var audience = _config["TokenSetting:Audience"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
                if (string.IsNullOrEmpty(tokenKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(Audience))
                {
          
                    throw new InvalidOperationException("JWT environment variables not configured properly.");
                }
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                var descriptor = new JwtSecurityToken(
                        issuer:issuer,
                        audience:audience,
                        expires:DateTime.UtcNow.AddDays(1),
                        signingCredentials:credentials,
                        claims:claim
                    );

                return new JwtSecurityTokenHandler().WriteToken(descriptor);
          

            }
            catch(Exception e) 
            {
                throw new Exception("Unable to create token");
            }
        }
    }
}
