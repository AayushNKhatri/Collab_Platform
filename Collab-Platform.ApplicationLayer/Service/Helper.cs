using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
        public (string userId, string role) GetTokenDetails()
        {
            try {
                var userID = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var role = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                return (userID, role);
            }
            catch (Exception e) {
                throw new Exception($"Errror {e}");
            }
        }
        public Guid GetProjectIDFormRequest() {
            var projectId = (_httpContext?.HttpContext?.GetRouteValue("ProjectId")) ?? throw new ArgumentException("Project Id is not there in route");
            return Guid.Parse(projectId.ToString());
        }
        public Guid GetTaskIdFormRequest()
        {
            var taskId = (_httpContext?.HttpContext?.GetRouteValue("TaskId")) ?? throw new ArgumentException("There is no Task id in route");
            return Guid.Parse(taskId.ToString());
        }
    }
}
