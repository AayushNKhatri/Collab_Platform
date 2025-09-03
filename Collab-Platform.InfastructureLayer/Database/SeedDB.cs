using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Collab_Platform.InfastructureLayer.Database
{
    public class SeedDB : ISeedService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModel> _userManager;
        public SeedDB(RoleManager<IdentityRole> roleManager, UserManager<UserModel> userManager) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task SeedRole()
        {
            try {
                var roles = new List<string>
                {
                    "Admin",
                    "User"
                };
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role)) {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                } 
            }
            catch (Exception ex) {
                throw new Exception($"Error:- {ex}");
            }
        }

        public async Task SeedAdmin()
        {
            try {
                string passowrd = "AdminPassword@123";
                var Admin = new UserModel
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    PhoneNumber = "9755422698",
                    EmailConfirmed = true,
                };
                var result = await _userManager.CreateAsync(Admin, passowrd);
                if (result.Succeeded) {
                    await _userManager.AddToRoleAsync(Admin, "Admin");
                }
            } 
            catch (Exception ex) 
            {
                throw new Exception($"Error:- {ex}"); 
            }
        }
    }
}
