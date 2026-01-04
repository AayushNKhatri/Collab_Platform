using Collab_Platform.ApplicationLayer.DTO.UserDto;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IAuthInterface
    {
        Task<string> Login(LoginDto loginDto);
    }
}
