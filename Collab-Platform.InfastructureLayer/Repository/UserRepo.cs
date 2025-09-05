using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.InfastructureLayer.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepo(UserManager<UserModel> userManager, ApplicationDbContext db, RoleManager<IdentityRole> roleManager){ 
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> CreateUser(UserModel user, string password)
        {
           return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteUser(UserModel user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<UserModel>> GetAllUser()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<UserModel> GetUserByID(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<string?> GetUserRole(UserModel user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        public async Task<IdentityResult> UpdateUser(UserModel user)
        {
            return await _userManager.UpdateAsync(user);
        }
        public async Task UpdateUserEmail(UserModel user) { 
               await _userManager.UpdateNormalizedEmailAsync(user);
        }
        public async Task<IdentityResult> UpdateUserPassword(UserModel user, string currentPassword ,string newPassword) {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
        public async Task<bool> UserExist(string? email, string? username)
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(username)){
                return await query.AnyAsync(u => u.Email == email && u.UserName == username);
            }
            if (!string.IsNullOrEmpty(email)){
                return await query.AnyAsync(u => u.Email == email);
            }
            if (!string.IsNullOrEmpty(username)){ 
                return await query.AnyAsync(u => u.UserName == username);
            }
            return false;
        }
    }
}
