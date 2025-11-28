using Collab_Platform.ApplicationLayer.DTO.ProjectRoleDTO;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface ICustomRolInterface
    {
        Task<ProjectRoleDetailDTO> GetAllCutomRoleByRoleID(Guid CustomRoleID);
        Task<List<ProjectRoleDetailDTO>> GetAllCustomRoleByProject(Guid ProjectID);

    }
}
