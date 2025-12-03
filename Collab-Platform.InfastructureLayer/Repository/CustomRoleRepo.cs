using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.InfastructureLayer.Database;
using Microsoft.EntityFrameworkCore;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class CustomRoleRepo : ICustomRoleRepo
    {
        private readonly ApplicationDbContext _db;
        public CustomRoleRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<CustomRoleModels>> GetAllCustomRoleByProject(Guid projectID)
        {
            return await _db.CustomRoleModels
                .Include(u => u.CustomRoleUsers)
                .Include(u => u.RolePermissions)
                .Where(u => u.ProjectId == projectID)
                .ToListAsync();
        }
        public async Task<CustomRoleModels?> GetCustomRoleByRoleID(Guid CustomRoleID)
        {
            return await _db.CustomRoleModels
              .Include(u => u.CustomRoleUsers)
              .ThenInclude(u => u.user)
              .Include(u => u.RolePermissions)
              .ThenInclude(u => u.Permission)
              .FirstOrDefaultAsync(u => u.CustomRoleId == CustomRoleID);
        }
        public async Task<List<CustomRoleModels>> GetCustomRoleByMultiID(List<Guid> CustomRoleID)
        {
            return await _db.CustomRoleModels.Include(u => u.CustomRoleUsers)
              .ThenInclude(u => u.user)
              .Include(u => u.RolePermissions)
              .ThenInclude(u => u.Permission)
              .Where(u => CustomRoleID.Contains(u.CustomRoleId))
              .ToListAsync();
        }
        public async Task AddCutomRole(CustomRoleModels roleModel)
        {
            _db.CustomRoleModels.Add(roleModel);
        }
        public async Task RemoveRole(CustomRoleModels roleModels)
        {
            _db.CustomRoleModels.Remove(roleModels);
        }
        public async Task AddUserToRole(List<CustomRoleUser> customRoleUsers)
        {
            await _db.RoleUsers.AddRangeAsync(customRoleUsers);
        }
        public async Task RemoveUserFormRole(List<CustomRoleUser> customRoleUsers) {
            _db.RoleUsers.RemoveRange(customRoleUsers);
        }
        public async Task AddPermissionToRole(List<RolePermissionModel> permission)
        {
            await _db.RolePermissions.AddRangeAsync(permission);
        }
        public async Task RemovePermissionFormRole(List<RolePermissionModel> permission)
        {
            _db.RolePermissions.RemoveRange(permission);
        }
    }
}
