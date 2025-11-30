using System.Runtime.CompilerServices;
using Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class CustomRoleService : ICustomRolInterface
    {
        public readonly ICustomRoleRepo _customRole;
        public readonly IPermissionRepo _permissionRepo;
        public readonly IHelperService _helperService;
        public readonly IProjectInterface _projectService;
        public readonly IUnitOfWork _unitOfWork;
        public CustomRoleService(ICustomRoleRepo customRole, IPermissionRepo permissionRepo, IHelperService helperService, IProjectInterface prjectService)
        {
            _customRole = customRole;
            _permissionRepo = permissionRepo;
            _helperService = helperService;
            _projectService = prjectService;
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
        public async Task CreateCutomeRole(CretaeCustomRoleDTO cretaeCustomRole, Guid ProjectID) //Need to add try catch and tranctaion
        { 
            var userID = _helperService.GetTokenDetails().userId;
            try
            {
            var project = await _projectService.GetProjectById(ProjectID) ?? throw new KeyNotFoundException("No project found with that id");
            await _unitOfWork.BeginTranctionAsync();
            var customRole = new CustomRoleModels
            {
                CustomRoleId = new Guid(),
                CustomRoleDesc = cretaeCustomRole.CustomRoleDesc,                 
                CustomRoleName = cretaeCustomRole.CustomRoleName,
                ProjectId = project.ProjectId,
                RoleCreatorId = userID,
            };
            await _customRole.AddCutomRole(customRole);
            await _unitOfWork.SaveChangesAsync();
            if (cretaeCustomRole.PermissionId.Count > 0)
            {
                var role = cretaeCustomRole.PermissionId.Select(u => new RolePermissionModel
                {
                    PermissionId = u,
                    CustomRoleId = customRole.CustomRoleId,
                }).ToList();
                await _permissionRepo.addPermissionToRole(role);
                await _unitOfWork.SaveChangesAsync();
            }
            if (cretaeCustomRole.UserId.Count > 0)
            {
                var user = cretaeCustomRole.UserId.Select( u => new CustomRoleUser
                {
                   UserID = u,
                   CustomRoleId = customRole.CustomRoleId 
                }).ToList();
                await _customRole.AddUserToRole(user);
                await _unitOfWork.SaveChangesAsync();
            }
                await _unitOfWork.CommitTranctionAsync();
            }
            catch
            {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }
            
        }   
    }
}
