using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using static Collab_Platform.PresentationLayer.Middleware.ExecptionClass;

namespace Collab_Platform.PresentationLayer.CustomAttribute
{
    public class ProjectAcessExtention : IAsyncActionFilter
    {

        private readonly IHelperService _helperService;
        private readonly ICustomRoleRepo _customRoleRepo;
        private readonly string _accessType;
        public ProjectAcessExtention(
            string accessType,
            ICustomRoleRepo customRoleRepo,
            IHelperService helperService
            ) 
        {
            _helperService = helperService;
            _customRoleRepo = customRoleRepo;
            _accessType = accessType;
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUser = _helperService.GetTokenDetails().userId;
            var projectId = _helperService.GetProjectIDFormRequest();
            var projectRole = await _customRoleRepo.GetRoleofUserInPorjetc(projectId, currentUser) ?? throw new InvalidRoleException("This User had not been Asingned Role");
            var permission = projectRole.SelectMany(u => u.RolePermissions).Select(u => u.Permission).ToList();
            if (!permission.Any(u => u.Key == _accessType))
            {
                throw new InvalidRoleException("This use had not been Assigned valid Role");
            }
            await next();
        }
    }
}
