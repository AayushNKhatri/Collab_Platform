namespace Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO
{
    public class UpdateCustomRoleDTO
    {
        public string CustomRoleName { get; set; }
        public string CustomRoleDesc { get; set; }

        public List<int>? PermissionId { get; set; }
        public List<string>? UserId { get; set; }
    }
}
