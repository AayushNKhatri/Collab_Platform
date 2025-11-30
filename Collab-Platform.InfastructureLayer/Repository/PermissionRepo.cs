using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.InfastructureLayer.Database;
using Microsoft.EntityFrameworkCore;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class PermissionRepo : IPermissionRepo
    {
        private readonly ApplicationDbContext _db;
        public PermissionRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<Permission>> GetAllPermission()
        {
            return await _db.Permissions.ToListAsync();
        }
        public async Task addPermissionToRole(List<RolePermissionModel> role)
        {
            await _db.RolePermissions.AddRangeAsync(role);
        }
    }
}