using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class EmailRepo : IEmailRepo
    {
        private readonly UserManager<UserModel> _userManager;
        public EmailRepo(UserManager<UserModel> userManager) {
            _userManager = userManager;
        }
        public async Task<UserModel> GetUserEmail(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            return user;
        }
    }
}
