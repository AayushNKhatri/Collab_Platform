using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class UserService : IUserInterface
    {
        public readonly IUserRepo _userRepo;
        public UserService(IUserRepo userRepo) { 
            _userRepo = userRepo;  
        }

        public async Task<IdentityResult> CreateUser(RegisterDto registerUser)
        {
            try 
            {
                var User = new UserModel { 
                    Email = registerUser.Email,
                    UserName = registerUser.UserName,
                    PhoneNumber = registerUser.PhoneNumber,
                };
                var UserExist = await _userRepo.UserExist(registerUser.Email, registerUser.UserName);
                if (UserExist) throw new Exception("This email or username is already in use");
                var result = await _userRepo.CreateUser(User, registerUser.Password);
                if (!result.Succeeded) throw new Exception($"The Error Occured{result.Errors}");
                return result;
            }
            catch(Exception e) 
            {
                throw new Exception("Error", e);
            }
        }
    }
}
