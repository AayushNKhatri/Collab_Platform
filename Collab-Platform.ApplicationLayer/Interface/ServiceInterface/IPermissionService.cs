using Collab_Platform.ApplicationLayer.DTO.PermissionDto;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IPermissionService
    {
        Task<List<ViewPermissionDTO>> GetAllPermission();
        Task <List<EndpointDTO>> GetContoller();
    }
}
