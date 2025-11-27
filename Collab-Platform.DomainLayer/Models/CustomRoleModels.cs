using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.DomainLayer.Models
{
    public class CustomRoleModels
    {
        public Guid CustomRoleId { get; set; }
        [Required]
        public string CustomRoleName { get; set; }
        public string CustomRoleDesc { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        public ProjectModel Project { get; set; }
        [Required]
        public string RoleCreatorId { get; set; }
        public UserModel RoleCreator { get; set; }
        public ICollection<UserModel> Users { get; set; } = new List<UserModel>();
        public ICollection<RolePermissionModel> RolePermissions { get; set; }
        public ICollection<CustomRoleUser> CustomRoleUsers { get; set; }

    }
}
