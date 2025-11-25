using System.Collections;

namespace Collab_Platform.DomainLayer.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Key { get; set; }
        public string name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public ICollection<RolePermissionModel> RolePermissions { get; set;}
    }
}