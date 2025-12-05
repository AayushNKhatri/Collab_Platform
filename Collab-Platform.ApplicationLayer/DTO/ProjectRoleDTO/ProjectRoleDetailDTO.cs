namespace Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO
{
    public class ProjectRoleDetailDTO
    {
        public Guid CustomRoleId { get; set; }
        public string CustomRoleName { get; set; }
        public string CustomRoleDesc { get; set; }
        public string RoleCreatorId { get; set; }
        public string RoleCreatorName { get; set;}
        public Guid ProjectID { get; set;}
        public string ProjectName { get; set;}
        public List<PermissionDTO>? permission { get; set;}
        public List<RoleUserDTO>? RoleUser { get; set;}

    }
    public class PermissionDTO
    {
        public int PermissionId { get; set;}
        public string PermissionKey { get; set;}
    }
    public class RoleUserDTO
    {
        public string UserID { get; set;}
        public string UserName { get; set;}
    }
}