using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(UserModel user); 
    }
}
