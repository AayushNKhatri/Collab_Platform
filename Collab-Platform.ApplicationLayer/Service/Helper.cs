using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class Helper : IHelperService
    {
        private readonly IHttpContextAccessor _httpContext;
        public Helper(IHttpContextAccessor httpContext) 
        {
            _httpContext = httpContext;
        }
        public (string, string) GetTokenDetails()
        {
            try {
                var userID = _httpContext.HttpContext.User.Identity.Name;
                var role = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                return (userID, role);
            }
            catch (Exception e) {
                throw new Exception($"Errror {e}");
            }
        }
    }
}
