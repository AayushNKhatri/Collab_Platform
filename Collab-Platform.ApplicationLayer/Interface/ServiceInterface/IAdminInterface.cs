using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IAdminInterface
    {
        Task<List<UserModel>> GetAllUserData();
    }
}
