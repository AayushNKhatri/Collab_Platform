using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Microsoft.AspNetCore.Mvc.Filters;
using static Collab_Platform.PresentationLayer.Middleware.ExecptionClass;

namespace Collab_Platform.PresentationLayer.CustomAttribute
{
    public class ProjectAcessExtention : IAsyncActionFilter
    {

        private readonly IHelperService _helperService;
        private readonly ICustomRoleRepo _customRoleRepo;
        private readonly IProjectRepo _projectRepo;
        private readonly ITaskRepo _taskRepo;
        private readonly string _accessType;
        public ProjectAcessExtention(
            string accessType,
            ICustomRoleRepo customRoleRepo,
            IHelperService helperService,
            IProjectRepo projectRepo,
            ITaskRepo taskRepo

            )
        {
            _helperService = helperService;
            _customRoleRepo = customRoleRepo;
            _projectRepo = projectRepo;
            _accessType = accessType;
            _taskRepo = taskRepo;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUser = _helperService.GetTokenDetails().userId;
            var projectId = _helperService.GetProjectIDFormRequest();
            var taskId = _helperService.GetTaskIdFormRequest();
            var task = await _taskRepo.GetTaskByTaskId(taskId);
            var project = await _projectRepo.GetProjectByID(projectId);
            if (project.CreatorId == currentUser)
            {
                await next();
            }
            if (taskId == Guid.Empty)
            {
                if (task.CreatedById == currentUser)
                {
                    await next();
                }
            }
            var projectRole = await _customRoleRepo.GetRoleofUserInPorjetc(projectId, currentUser) ?? throw new InvalidRoleException("This User had not been Asingned Role");
            var permission = projectRole.SelectMany(u => u.RolePermissions).Select(u => u.Permission).ToList();
            if (!permission.Any(u => u.Key == _accessType))
            {
                throw new InvalidRoleException("This user had not been Assigned valid Role");
            }
            await next();
        }
    }
}
