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
        public readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepo userRepo, IUnitOfWork unitOfWork) { 
            _userRepo = userRepo;  
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityResult> CreateUser(RegisterDto registerUser)
        {
            try 
            {
                var UserExist = await _userRepo.UserExist(registerUser.Email, registerUser.UserName);
                if (UserExist) throw new InvalidOperationException($"User already exist with this username and email");
                var User = new UserModel { 
                    Email = registerUser.Email,
                    UserName = registerUser.UserName,
                    PhoneNumber = registerUser.PhoneNumber,
                };
                await _unitOfWork.BeginTranctionAsync();
                var result = await _userRepo.CreateUser(User, registerUser.Password);
                var user = await _userRepo.GetUserByID(User.Id);
                if (user == null) { 
                    await _unitOfWork.RollBackTranctionAsync();
                    throw new InvalidOperationException("User cannot be register contatct admin");
                }
                if (!result.Succeeded) throw new ArgumentException($"The Error Occured{result.Errors.Select(e=>e.Description)}");
                var role = "User";
                var roleResult = await _userRepo.AddUserRole(user, role);
                if (roleResult == null) 
                {
                    await _unitOfWork.RollBackTranctionAsync();
                    throw new InvalidOperationException("Connot assing the role Registration failed");
                }
                await _unitOfWork.CommitTranctionAsync();
                return result;
            }
            catch(Exception e) 
            {
                throw;
            }
        }
        public async Task<UserProfileDto> UserProfile(string userId) {
            try {
                var user = await _userRepo.GetUserByID(userId) ?? throw new KeyNotFoundException("User Not found");
                var UserProfile = new UserProfileDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    isEmailConfirmed = user.EmailConfirmed,

                };
                return UserProfile;
                
            }
            catch (Exception ex) { 
                throw;
            }
        }
        public async Task DeleteUserById(string userID) {
            try
            {
                var user = await _userRepo.GetUserByID(userID) ?? throw new KeyNotFoundException("User Not Found");
                var result = await _userRepo.DeleteUser(user);
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<IdentityResult> UpdateUserProfile(UpdateUserDTO updateUser, string userID)
        {
            try {
                var user = await _userRepo.GetUserByID(userID) ?? throw new KeyNotFoundException("No User find with given id");
                if(!string.IsNullOrEmpty(updateUser.UserName))
                    user.UserName = updateUser.UserName;
                if (!string.IsNullOrEmpty(updateUser.Email))
                    await _userRepo.UpdateUserEmail(user);
                if (!string.IsNullOrEmpty(updateUser.newPassword)) 
                {
                    var cPassword = updateUser.currentPassword ?? throw new ArgumentException("Please provide current password if you want to update password");
                    await _userRepo.UpdateUserPassword(user, cPassword, updateUser.currentPassword);
                }
                if(!string.IsNullOrEmpty(updateUser.PhoneNumber))
                     user.UserName =updateUser.PhoneNumber;
              
                var result = await _userRepo.UpdateUser(user);
                return result;
                   
                //Later i might have to refator this for adjusting Email Service and make different email update service
            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}
