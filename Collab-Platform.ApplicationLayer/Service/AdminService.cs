using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class AdminService : IAdminInterface
    {
        public readonly IUserRepo _userRepo;
        public AdminService(IUserRepo userRepo) {
            _userRepo = userRepo;
        }
        public async Task<List<UserModel>> GetAllUserData() {
            try {
                var Users = await _userRepo.GetAllUser();
                return Users;
            }
            catch(Exception e) {
                throw new Exception(e.Message);
            }
        }
    }
}
