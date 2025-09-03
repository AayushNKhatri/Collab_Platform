using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface IUserRepo
    {
        Task<IdentityResult> CreateUser(UserModel user, string password);
        Task<UserModel> GetUserByID(string userId);
        Task<List<UserModel>> GetAllUser();
        Task<IdentityResult> UpdateUser(UserModel user);
        Task<IdentityResult> DeleteUser(UserModel user);
        Task<string?> GetUserRole(UserModel user);
        Task<bool> UserExist(string email, string username);
    }
}
