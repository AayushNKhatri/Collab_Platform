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
        public Task<List<Permission>> GetAllPermission()
        {
            return _db.Permissions.ToListAsync();
        }
    }
}