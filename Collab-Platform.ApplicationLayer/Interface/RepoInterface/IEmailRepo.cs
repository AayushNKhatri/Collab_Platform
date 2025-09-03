using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface IEmailRepo
    {
        Task <UserModel> GetUserEmail(string email);
    }
}
