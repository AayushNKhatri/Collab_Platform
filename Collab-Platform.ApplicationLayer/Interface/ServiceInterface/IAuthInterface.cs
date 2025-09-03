using Collab_Platform.ApplicationLayer.DTO.UserDto;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IAuthInterface
    {
        Task<string> Login(LoginDto loginDto);
    }
}
