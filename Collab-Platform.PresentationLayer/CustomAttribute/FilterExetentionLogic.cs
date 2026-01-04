using Collab_Platform.ApplicationLayer.Interface.HelperInterface;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Microsoft.AspNetCore.Mvc.Filters;
using static Collab_Platform.PresentationLayer.Middleware.ExecptionClass;

namespace Collab_Platform.PresentationLayer.CustomAttribute
{
    public class ProjectAcessExtention : IAsyncActionFilter
    {

        private readonly IDataHelper _helperService;
        private readonly ICustomRoleRepo _customRoleRepo;
        private readonly IProjectRepo _projectRepo;
        private readonly ITaskRepo _taskRepo;
        private readonly IChannelRepo _channelRepo;
        private readonly string _accessType;
        public ProjectAcessExtention(
            string accessType,
            ICustomRoleRepo customRoleRepo,
            IDataHelper helperService,
            IProjectRepo projectRepo,
            ITaskRepo taskRepo,
            IChannelRepo channelRepo

            )
        {
            _helperService = helperService;
            _customRoleRepo = customRoleRepo;
            _projectRepo = projectRepo;
            _accessType = accessType;
            _taskRepo = taskRepo;
            _channelRepo = channelRepo;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var currentUser = _helperService.GetTokenDetails().userId;
            var routeData = _helperService.GetRouteData();
            if(routeData.ProjectId is Guid projectId)
            {
                var project = await _projectRepo.GetProjectByID(projectId);
                if(project.CreatorId == currentUser)
                {
                    await next();
                    return;
                }
            }
            if(routeData.TaskId is Guid taskId)
            {
                var task = await _taskRepo.GetTaskByTaskId(taskId);
                if(task.CreatedById == currentUser)
                {
                    await next();
                    return;
                }
            }
            if (routeData.ChannelId is Guid ChannelId) {
                var channel = await _channelRepo.GetChannelByID(ChannelId);
                if (channel.CreatorId == currentUser) 
                {
                    await next();
                    return;
                }
            }
            if(routeData.ProjectId is not Guid ProjectId)
            {
                throw new InvalidOperationException("There is no project id for this HTTP context");
            }
            var projectRole = await _customRoleRepo.GetRoleofUserInPorjetc(ProjectId, currentUser) ?? throw new InvalidRoleException("This User had not been Asingned Role");
            var permission = projectRole.SelectMany(u => u.RolePermissions).Select(u => u.Permission).ToList();
            if (!permission.Any(u => u.Key == _accessType))
            {
                throw new InvalidRoleException("This user had not been Assigned valid Role");
            }
            await next();
        }
    }
}
