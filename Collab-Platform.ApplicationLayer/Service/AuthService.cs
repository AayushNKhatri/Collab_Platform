using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class AuthService : IAuthInterface
    {
        public readonly SignInManager<UserModel> _signInManager;
        public readonly IEmailRepo _emailRepo;
        public readonly ITokenService _tokenService;
        public AuthService(SignInManager<UserModel> signInManager, IEmailRepo emailRepo, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _emailRepo = emailRepo;
            _tokenService = tokenService;
        }
        public async Task<string> Login(LoginDto loginDto)
        {
            try {
                //find the user
                var user = await _emailRepo.GetUserEmail(loginDto.email) ?? throw new UnauthorizedAccessException("Invalid Email or password");
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.password, false) ?? throw new UnauthorizedAccessException("Invalid Email or password");
                var token = await _tokenService.GenerateJwtToken(user);
                return token;
            }
            catch(Exception e) {
                throw new Exception($"Error:-{e}");
            }
            
        }
    }
}
