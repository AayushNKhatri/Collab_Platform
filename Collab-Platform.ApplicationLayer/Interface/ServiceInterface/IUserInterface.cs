using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IUserInterface
    {
        Task<IdentityResult> CreateUser(RegisterDto registerUser);
        Task<UserProfileDto> UserProfile(string userId);
        Task DeleteUserById(string userID);
        Task<IdentityResult> UpdateUserProfile(UpdateUserDTO updateUser, string userId);
    }
}
