using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface ICustomRoleRepo
    {
        Task<List<CustomRoleModels>> GetAllCustomRoleByProject(Guid projectID);
        Task<CustomRoleModels?> GetCustomRoleByRoleID(Guid CustomRoleID);
        Task AddCutomRole(CustomRoleModels roleModel);
        Task RemoveRole(CustomRoleModels roleModels);
        Task AddUserToRole(List<CustomRoleUser> roleUser);
        Task RemoveUserFormRole(List<CustomRoleUser> customRoleUsers);
        Task AddPermissionToRole(List<RolePermissionModel> permission);
        Task RemovePermissionFormRole(List<RolePermissionModel> permission);
        Task<List<CustomRoleModels>> GetRoleofUserInPorjetc(Guid projectId, string UserID);
    }
}
