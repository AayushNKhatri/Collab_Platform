using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.HelperInterface
{
    public interface IIdentityService
    {
        Task<string> GenerateJwtToken(UserModel user);
    }
}
