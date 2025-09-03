using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Microsoft.AspNetCore.Identity;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IUserInterface
    {
        Task<IdentityResult> CreateUser(RegisterDto registerUser);
    }
}
