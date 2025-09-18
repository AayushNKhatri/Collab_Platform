using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class ProjectService : IProjectInterface
    {
        private readonly IProjectRepo _projectRepo;
        private readonly IHelperService _helperService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepo _userRepo;
        public ProjectService(IProjectRepo projectRepo, IHelperService helperService,IUnitOfWork unitOfWork, IUserRepo userRepo ) { 
            _projectRepo = projectRepo;
            _helperService = helperService;
            _unitOfWork = unitOfWork;
            _userRepo = userRepo;
        }

        public async Task AddUserToProject(AddUserProjectDto addUserProject)
        {
            var projectId = addUserProject.ProjectId;
            var project = await _projectRepo.GetProjectByID(projectId) ?? throw new KeyNotFoundException("No project found with tha id");
            var userId = addUserProject.UserId;
            var users = await _userRepo.GetMultipeUserById(userId);
            if (users == null || !users.Any())
                throw new KeyNotFoundException("No users found for given IDs");
            var userProject = users.Select(u => new UserProject
            {
                ProjectId = projectId,
                UserId = u.Id,
            }).ToList();
            await _projectRepo.addUserToProject(userProject);
        }

        public async Task<ProjectModel> CreateProject(CreateProjectDto createProjectDto)
        {
            try {
                var userID = _helperService.GetTokenDetails().Item1 ?? throw new KeyNotFoundException("UserID not found");
                await _unitOfWork.BeginTranctionAsync();
                var project = new ProjectModel {
                    ProjectId = Guid.NewGuid(),
                    ProjectName = createProjectDto.ProjectName,
                    PorjectVisibility = createProjectDto.PorjectVisibility,
                    ProjectDesc = createProjectDto.ProjectDesc,
                    CreatorId = userID,
                    StartedAt = DateTime.UtcNow,
                    EstComplete = createProjectDto.EstComplete,
                    ActualComplete = null,
                    UserProjects = new List<UserProject>()
                };
                var result = await _projectRepo.CreateProject(project);
                var useProject = new List<UserProject>
                {
                    new UserProject{
                    ProjectId = project.ProjectId,
                    UserId = userID,
                    }
                };
                await _projectRepo.addUserToProject(useProject);
                if (createProjectDto.ProjectMemberID != null)
                {
                    var projectMember = createProjectDto.ProjectMemberID.Distinct().Select(u => new UserProject
                    {
                        ProjectId = project.ProjectId,
                        UserId = u,
                    }).ToList();
                    await _projectRepo.addUserToProject(projectMember);
                }

                var saveResult = await _unitOfWork.SaveChangesAsync();
                if (saveResult <= 0)
                {
                    await _unitOfWork.RollBackTranctionAsync();
                    throw new Exception("Project was not saved to DB, transaction aborted.");
                }
                await _unitOfWork.CommitTranctionAsync();
                return result;
            }
            catch{
                throw;
            }
        }

        public async Task<List<ProjectDetailDto>> GetAllProject()
        {
            var result = await _projectRepo.GetAllProject();
            var project = result.Select(p => new ProjectDetailDto { 
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                ProjectDesc = p.ProjectDesc,
                CreatorId = p.CreatorId,
                Creator = p.Creator,
                PorjectVisibility = p.PorjectVisibility,
                InviteCode = p.InviteCode,
                StartedAt = p.StartedAt,
                EstComplete = p.EstComplete,
                ActualComplete = p.ActualComplete,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                UserProjects = p.UserProjects,
                Tasks = p.Tasks,
                Channel = p.Channel,
                CustomRoles = p.CustomRoles,
            }).ToList();
            return project;
        }

        public async Task<ProjectDetailDto> GetProjectById(Guid projectId)
        {
            var result = await _projectRepo.GetProjectByID(projectId) ?? throw new KeyNotFoundException("No Project found with that id");
            var project = new ProjectDetailDto
            {
                ProjectId = result.ProjectId,
                ProjectName = result.ProjectName,
                ProjectDesc = result.ProjectDesc,
                CreatorId = result.CreatorId,
                Creator = result.Creator,
                PorjectVisibility = result.PorjectVisibility,
                InviteCode = result.InviteCode,
                StartedAt = result.StartedAt,
                EstComplete = result.EstComplete,
                ActualComplete = result.ActualComplete,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt,
                UserProjects = result.UserProjects,
                Tasks = result.Tasks,
                Channel = result.Channel,
                CustomRoles = result.CustomRoles,
            };
            return project;
        }

        public async Task<List<ProjectDetailDto>> GetProjectByUserId()
        {
            var userId = _helperService.GetTokenDetails().Item1 ?? throw new KeyNotFoundException("UserID not found");
            var result = await _projectRepo.GetAllProjectByUserID(userId) ?? throw new KeyNotFoundException("No project found for given user");
            var project = result.Select( p => new ProjectDetailDto{
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                ProjectDesc = p.ProjectDesc,
                CreatorId = p.CreatorId,
                Creator = p.Creator,
                PorjectVisibility = p.PorjectVisibility,
                InviteCode = p.InviteCode,
                StartedAt = p.StartedAt,
                EstComplete = p.EstComplete,
                ActualComplete = p.ActualComplete,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                UserProjects = p.UserProjects,
                Tasks = p.Tasks,
                Channel = p.Channel,
                CustomRoles = p.CustomRoles,
            }).ToList();
            return project;
        }
    }
}
