using Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class CustomRoleService : ICustomRolInterface
    {
        public readonly ICustomRoleRepo _customRole;
        public readonly IPermissionRepo _permissionRepo;
        public CustomRoleService(ICustomRoleRepo customRole, IPermissionRepo permissionRepo)
        {
            _customRole = customRole;
            _permissionRepo = permissionRepo;
        }

        public async Task<ProjectRoleDetailDTO> GetAllCutomRoleByRoleID(Guid CustomRoleID)
        {
            var customRole = await _customRole.GetCustomRoleByRoleID(CustomRoleID) ?? throw new KeyNotFoundException("Custom role Not found with that id");
            var pemission = customRole.RolePermissions.Select( u => u.Permission);
            var user = customRole.CustomRoleUsers.Select(u => u.user);
            var response = new ProjectRoleDetailDTO
            {
                CustomRoleId = customRole.CustomRoleId,
                CustomRoleDesc = customRole.CustomRoleDesc,
                CustomRoleName = customRole.CustomRoleName,
                ProjectID = customRole.ProjectId,
                ProjectName = customRole.Project.ProjectName,
                RoleCreatorId = customRole.RoleCreatorId,
                RoleCreatorName = customRole.RoleCreator.UserName,
                permission = pemission.Select(u => new PermissionDTO
                {
                    PermissionId = u.PermissionId,
                    PermissionKey = u.Key,
                }).ToList(),
                RoleUser = user.Select(u => new RoleUserDTO
                {
                    UserID = u.Id,
                    UserName = u.UserName
                }).ToList()
            };
            return response; 
        }
        public async Task<List<ProjectRoleDetailDTO>> GetAllCustomRoleByProject (Guid ProjectID)
        {
            var customRole = await _customRole.GetAllCustomRoleByProject(ProjectID);
            var ProjectRoleDetail = customRole.Select(u => new ProjectRoleDetailDTO
            {
                CustomRoleId = u.CustomRoleId,
                CustomRoleDesc = u.CustomRoleDesc,
                CustomRoleName = u.CustomRoleName,
                ProjectID = u.ProjectId,
                ProjectName = u.Project.ProjectName,
                RoleCreatorId = u.RoleCreatorId,
                RoleCreatorName = u.RoleCreator.UserName,
                permission = u.RolePermissions.Select(u => 
                    new PermissionDTO
                    {
                        PermissionId = u.Permission.PermissionId,
                        PermissionKey = u.Permission.Key,
                    }
                ).ToList(),
                RoleUser = u.CustomRoleUsers.Select(u => new RoleUserDTO { 
                    UserID = u.user.Id,
                    UserName = u.user.UserName
                }).ToList()
            }).ToList();
            return ProjectRoleDetail; 
        }
        public Task CreateCutomeRole(CretaeCustomRoleDTO cretaeCustomRole, Guid ProjectID)
        { 
                 
        }
    }
}
