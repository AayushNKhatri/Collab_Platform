namespace Collab_Platform.DomainLayer.Models
{
    public class RolePermissionModel
    {
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
        public Guid CustomRoleId { get; set; }
        public CustomRoleModels CustomRole { get; set; }        
    }
}