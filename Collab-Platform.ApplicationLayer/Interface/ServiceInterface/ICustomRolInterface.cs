using Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface ICustomRolInterface
    {
        Task<ProjectRoleDetailDTO> GetAllCutomRoleByRoleID(Guid CustomRoleID);
        Task<List<ProjectRoleDetailDTO>> GetAllCustomRoleByProject(Guid ProjectID);
        Task CreateCutomeRole(CretaeCustomRoleDTO createCustomRole, Guid ProjectID);
        Task DeleteCustomRole(Guid RoleId);
        Task UpdateCustomRole(Guid RoleID, UpdateCustomRoleDTO updateCustomRole);
        Task RemoveUserFromRole(List<string> UserId, Guid CustomRoleId);
        Task AddUserToRole(List<string> UserId, Guid CustomRoleID);
        Task RemovePermissionFormRole(List<int> PermissionId, Guid CustomeRoleId);
        Task AddPermissionToRole(List<int> PermissionId, Guid CustomRoleID);
    }
}
