using Collab_Platform.ApplicationLayer.DTO.PermissionDto;
using System.Reflection;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IPermissionService
    {
        Task<List<ViewPermissionDTO>> GetAllPermission();
        Task<List<Type>> GetContoller(Assembly asm);
    }
}
