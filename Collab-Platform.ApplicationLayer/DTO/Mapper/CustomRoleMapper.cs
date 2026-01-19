using Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.DTO.Mapper
{
    public static class CustomRoleMapper
    {
        public static ProjectRoleDetailDTO ToProjectRole(CustomRoleModels customRole) {
            return new ProjectRoleDetailDTO
            {
                CustomRoleId = customRole.CustomRoleId,
                CustomRoleName = customRole.CustomRoleName,
                CustomRoleDesc = customRole.CustomRoleDesc,
                ProjectID = customRole.ProjectId,
                ProjectName = customRole.Project.ProjectName,
                RoleCreatorId = customRole.RoleCreatorId,
                RoleCreatorName = customRole.RoleCreator.UserName,
                permission = customRole.RolePermissions.Select(u => new PermissionDTO
                {
                    PermissionId = u.PermissionId,
                    PermissionKey = u.Permission.Key,
                }).ToList(),
                RoleUser = customRole.CustomRoleUsers.Select(u => new RoleUserDTO { 
                    UserID = u.UserID,
                    UserName = u.user.UserName
                }).ToList()
            };
        }
        public static List<ProjectRoleDetailDTO> ToListProjectRole(List<CustomRoleModels> customRole)
        {
            var list = customRole.Select(customRole => new ProjectRoleDetailDTO
            {
                CustomRoleId = customRole.CustomRoleId,
                CustomRoleName = customRole.CustomRoleName,
                CustomRoleDesc = customRole.CustomRoleDesc,
                ProjectID = customRole.ProjectId,
                ProjectName = customRole.Project.ProjectName,
                RoleCreatorId = customRole.RoleCreatorId,
                RoleCreatorName = customRole.RoleCreator.UserName,
                permission = customRole.RolePermissions.Select(u => new PermissionDTO
                {
                    PermissionId = u.PermissionId,
                    PermissionKey = u.Permission.Key,
                }).ToList(),
                RoleUser = customRole.CustomRoleUsers.Select(u => new RoleUserDTO
                {
                    UserID = u.UserID,
                    UserName = u.user.UserName
                }).ToList()
            }).ToList();
            return list;
        }
    }
}
