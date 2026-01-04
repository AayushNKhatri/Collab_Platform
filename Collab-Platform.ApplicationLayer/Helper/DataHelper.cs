using Collab_Platform.ApplicationLayer.Interface.HelperInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Collab_Platform.ApplicationLayer.Helper
{
    public class DataHelper : IDataHelper
    {
        private readonly IHttpContextAccessor _httpContext;
        public DataHelper(IHttpContextAccessor httpContext) 
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
        public (Guid? ProjectId, Guid? TaskId, Guid? ChannelId) GetRouteData()
        {
            Guid? ProjectId = null;
            Guid? TaskId = null;
            Guid? ChannelId = null;
            var projectIdValue = _httpContext?.HttpContext?.GetRouteValue("ProjectId")?.ToString();
            var taskIdValue = _httpContext?.HttpContext?.GetRouteValue("TaskId")?.ToString();
            var ChannelValue = _httpContext?.HttpContext?.GetRouteValue("ChannelId")?.ToString();

            if(Guid.TryParse(projectIdValue, out var pid))  //This line check if projectIdValue is parsanable if not then retrun and do not assing any thing to ProjectId which return null
            {                                                             //If the value exist then if will parsen to Guid and return the value which is assiged to pid and will be set in   
                ProjectId = pid;                                                   //Project Id here
            }
            if(Guid.TryParse(taskIdValue, out var tid))
            {
                TaskId = tid;
            }
            if (Guid.TryParse(ChannelValue, out var cid)) {
                ChannelId = cid;
            }
            return (ProjectId, TaskId, ChannelId);
        }
    }
}
