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
            var pemission = customRole.RolePermissions.Select(u => u.Permission);
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
        public async Task<List<ProjectRoleDetailDTO>> GetAllCustomRoleByProject(Guid ProjectID)
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
                RoleUser = u.CustomRoleUsers.Select(u => new RoleUserDTO
                {
                    UserID = u.user.Id,
                    UserName = u.user.UserName
                }).ToList()
            }).ToList();
            return ProjectRoleDetail;
        }
        public async Task CreateCutomeRole(CretaeCustomRoleDTO createCustomRole, Guid ProjectID)
        {
            var userID = _helperService.GetTokenDetails().userId;
            try
            {
                var project = await _projectService.GetProjectById(ProjectID) ?? throw new KeyNotFoundException("No project found with that id");
                await _unitOfWork.BeginTranctionAsync();
                var customRole = new CustomRoleModels
                {
                    CustomRoleId = new Guid(),
                    CustomRoleDesc = createCustomRole.CustomRoleDesc,
                    CustomRoleName = createCustomRole.CustomRoleName,
                    ProjectId = project.ProjectId,
                    RoleCreatorId = userID,
                };
                await _customRole.AddCutomRole(customRole);
                if (createCustomRole.PermissionId.Count > 0)
                {
                    var role = createCustomRole.PermissionId.Select(u => new RolePermissionModel
                    {
                        PermissionId = u,
                        CustomRoleId = customRole.CustomRoleId,
                    }).ToList();
                    await _permissionRepo.addPermissionToRole(role);
                }
                if (createCustomRole.UserId.Count > 0)
                {
                    var user = createCustomRole.UserId.Select(u => new CustomRoleUser
                    {
                        UserID = u,
                        CustomRoleId = customRole.CustomRoleId
                    }).ToList();
                    await _customRole.AddUserToRole(user);
                }
                await _unitOfWork.CommitTranctionAsync();
            }
            catch
            {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }
        }

        public async Task DeleteCustomRole(Guid RoleId)
        {
            var customRole = await _customRole.GetCustomRoleByRoleID(RoleId) ?? throw new KeyNotFoundException("This Role name does not exist with this id");
            await _customRole.RemoveRole(customRole);
        }
        public async Task UpdateCustomRole(Guid RoleID, UpdateCustomRoleDTO updateCustomRole)
        {
            try 
            {
                await _unitOfWork.BeginTranctionAsync();
                var customRole = await _customRole.GetCustomRoleByRoleID(RoleID) ?? throw new KeyNotFoundException("This role does not exist");
                var customRoleUser = customRole.CustomRoleUsers.Select(u => u.UserID).ToHashSet();
                var memberToRemove = customRoleUser;
                var customRolePermission = customRole.RolePermissions.Select(u => u.PermissionId).ToHashSet();
                customRole.CustomRoleDesc = updateCustomRole.CustomRoleDesc;
                customRole.CustomRoleName = updateCustomRole.CustomRoleName;
                if (updateCustomRole.UserId.Count != 0)
                {
                    var toBeUpdateUser = new HashSet<string>(updateCustomRole.UserId);
                    memberToRemove.ExceptWith(toBeUpdateUser);
                    toBeUpdateUser.ExceptWith(customRoleUser);
                    if (toBeUpdateUser.Count != 0)
                    {
                        var updateUser = toBeUpdateUser.Select(u => new CustomRoleUser
                        {
                            CustomRoleId = customRole.CustomRoleId,
                            UserID = u
                        }).ToList();
                        await _customRole.AddUserToRole(updateUser);
                    }
                    if (memberToRemove.Count != 0)
                    {
                        var removeUser = customRole.CustomRoleUsers.Where(u => memberToRemove.Contains(u.UserID)).ToList();
                        await _customRole.RemoveRole(customRole);
                    }
                }
                await _unitOfWork.CommitTranctionAsync();
            }
            catch 
            {
                await _unitOfWork.RollBackTranctionAsync(); 
                throw;
            }


        }

        public async Task AddUserToRole(List<string> UserId, Guid CustomRoleID) { 
            var CustomRole = await _customRole.GetCustomRoleByRoleID(CustomRoleID) ?? throw new KeyNotFoundException("Project with given id does not exist");
            var existingUser = CustomRole.CustomRoleUsers.Select(u => u.UserID).ToHashSet();
            var ToBeAddUser = UserId.ToHashSet();
            ToBeAddUser.ExceptWith(existingUser);   
            var userToAdd = ToBeAddUser.Select(u => new CustomRoleUser
            {
               UserID = u,
               CustomRoleId = CustomRole.CustomRoleId 
            }).ToList();
            await _customRole.AddUserToRole(userToAdd);
        }
        public async Task RemoveUserFromRole(List<string> UserId, Guid CustomRoleId){
          var CustomRole = await _customRole.GetCustomRoleByRoleID(CustomRoleId) 
            ?? throw new KeyNotFoundException("There is no role with that id");

          var existingUser = CustomRole.CustomRoleUsers.Select(u => u.UserID).ToHashSet();
          var TobeRemove = UserId.ToHashSet();
          TobeRemove.ExceptWith(existingUser);
          var userToRemove = TobeRemove.Select(u => new CustomRoleUser
          {
            CustomRoleId  = CustomRole.CustomRoleId,
            UserID = u
          }).ToList();
          await _customRole.RemoveUserFormRole(userToRemove);
        }
        public async Task RemovePermissionFormRole(List<int> PermissionId, Guid CustomeRoleId)
        {
            var customRole = await _customRole.GetCustomRoleByRoleID(CustomeRoleId) ?? throw new KeyNotFoundException("There is no role wtih this id");
            var existingPermission = customRole.RolePermissions.Select(u => u.PermissionId).ToHashSet();
            var RemovePermission = PermissionId.ToHashSet();
            RemovePermission.ExceptWith(existingPermission);
            var PermissionToRemove = RemovePermission.Select(u => new RolePermissionModel
            {
               CustomRoleId = customRole.CustomRoleId,
               PermissionId  = u 
            }).ToList();
            await _customRole.RemovePermissionFormRole(PermissionToRemove);
        }
        public async Task AddPermissionToRole(List<int> PermissionId, Guid CustomRoleID)
        {
            var customRole = await _customRole.GetCustomRoleByRoleID(CustomRoleID) ?? throw new KeyNotFoundException("There is no role with this id");
            var existingPermission = customRole.RolePermissions.Select(u => u.PermissionId).ToHashSet();
            var AddPermission = PermissionId.ToHashSet();
            AddPermission.ExceptWith(existingPermission);
            var PermissionToAdd = AddPermission.Select(u => new RolePermissionModel
            {
                CustomRoleId = customRole.CustomRoleId,
                PermissionId = u
            }).ToList();
            await _customRole.AddPermissionToRole(PermissionToAdd);
        }
    }
}
