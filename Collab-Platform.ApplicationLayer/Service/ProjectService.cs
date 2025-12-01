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

        public async Task AddUserToProject(Guid projectId, List<string> UserId)
        {
            var userId = _helperService.GetTokenDetails().userId ?? throw new KeyNotFoundException("UserID not found");
            var project = await _projectRepo.GetProjectByID(projectId) ?? throw new KeyNotFoundException("No project found with given id");
            if (project.CreatorId != userId)
                throw new InvalidOperationException("User must be the creator of this project");
            var userIdToBeAdd = UserId;
            var users = await _userRepo.GetMultipeUserById(userIdToBeAdd);
            if (users == null || !users.Any())
                throw new KeyNotFoundException("No users found for given IDs");
            if (project.UserProjects.Any(x => x.UserId == userId)) throw new InvalidOperationException("This user is already in the project");
            var userProject = users.Select(u => new UserProject
            {
                ProjectId = projectId,
                UserId = u.Id,
            }).ToList();
            await _projectRepo.addUserToProject(userProject);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ProjectModel> CreateProject(CreateProjectDto createProjectDto)
        {
            try {
                var userID = _helperService.GetTokenDetails().userId ?? throw new KeyNotFoundException("UserID not found");
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
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }
        }

        public async Task<List<ProjectDetailDto>> GetAllProject()
        {
            var result = await _projectRepo.GetAllProject();
            return result.Select(project => new ProjectDetailDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                CreatorName = project.Creator.UserName,
                ActualComplete = project.ActualComplete,
                PorjectVisibility = project.PorjectVisibility,
                InviteCode = project.InviteCode,
                ProjectDesc = project.ProjectDesc,
                CreatedAt = project.CreatedAt,
                EstComplete = project.EstComplete,
                StartedAt = project.StartedAt,
                UpdatedAt = project.UpdatedAt,
                UserDetails = project.UserProjects.Select(up => new UserProjectDetailsDto
                {
                    UserId = up.UserId,
                    Username = up.User.UserName
                }).ToList()
            }).ToList();
        }

        public async Task RemoveUserFormProject( Guid projectId, List<string> UserID)
        {
            var userPorject = await _projectRepo.GetUserProjectByUserIDs(UserID, projectId);
            if(!userPorject.Any())
                throw new  KeyNotFoundException("No user found with that id");
            _projectRepo.deleteUserProject(userPorject);
        }

        public async Task<ProjectDetailDto> GetProjectById(Guid projectId)
        {
            var result = await _projectRepo.GetProjectByID(projectId) ?? throw new KeyNotFoundException("No Project found with that id");
            var userDetails = await GetUserProjectDetails(result);
            var project = new ProjectDetailDto
            {
                ProjectId = result.ProjectId,
                ProjectName = result.ProjectName,
                ProjectDesc = result.ProjectDesc,
                CreatorName = result.Creator.UserName,
                PorjectVisibility = result.PorjectVisibility,
                InviteCode = result.InviteCode,
                StartedAt = result.StartedAt,
                EstComplete = result.EstComplete,
                ActualComplete = result.ActualComplete,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt,
                UserDetails = userDetails
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
                CreatorName = p.Creator.UserName,
                PorjectVisibility = p.PorjectVisibility,
                InviteCode = p.InviteCode,
                StartedAt = p.StartedAt,
                EstComplete = p.EstComplete,
                ActualComplete = p.ActualComplete,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                UserDetails = p.UserProjects.Select(u => new UserProjectDetailsDto
                {
                    UserId = u.UserId,
                    Username = u.User.UserName
                }).ToList()
            }).ToList();
            return project;
        }

        public async Task<ProjectDetailDto> UpdateProject(Guid projectID, UpdateProjectDto updateProject)
        {
            if (updateProject == null)
                throw new ArgumentNullException("Update Project is required to update the project.");

            try
            {
                await _unitOfWork.BeginTranctionAsync();
                var projectModel = await _projectRepo.GetProjectByID(projectID) ?? throw new KeyNotFoundException("Project Not found");
                var existingUser = projectModel.UserProjects
                    .Where(u => u.UserId != projectModel.CreatorId)
                    .Select(u => u.UserId).ToHashSet();
                var incomingUser = new HashSet<string>(updateProject.ProjectMemberID);
                var membersToRemove = new HashSet<string>(existingUser);
                membersToRemove.ExceptWith(incomingUser);           //filtering users that needs to be removed
                incomingUser.ExceptWith(existingUser);              //filtering users that needs to be add
                if (incomingUser.Any()) //First get userproject detail through id then remove The User Form Project same goes for add
                { 
                    var userPorjectToAdd = await MapUserProject(incomingUser, projectID);
                    await _projectRepo.addUserToProject(userPorjectToAdd);
                }
                if (membersToRemove.Any())
                {
                    var remove= projectModel.UserProjects.Where(u => membersToRemove.Contains(u.UserId)).ToList(); //The reason not using mapp user project is the behviour between add and remove we need to use already loaded db for to remove or update the db so we mapped diretly
                    await _projectRepo.deleteUserProject(remove); // Ef try ro cretae new row if i do map new so it will conflict
                }
                projectModel.ProjectName = updateProject.ProjectName; 
                projectModel.ProjectDesc = updateProject.ProjectDesc;
                projectModel.EstComplete = updateProject.EstComplete;
                projectModel.PorjectVisibility = updateProject.PorjectVisibility;     
                projectModel.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTranctionAsync();

                var userDetails = await GetUserProjectDetails(projectModel);
                var projectDetail = new ProjectDetailDto
                {
                    ProjectId = projectModel.ProjectId, 
                    ProjectName = projectModel.ProjectName,
                    ProjectDesc = projectModel.ProjectDesc,
                    CreatorName = projectModel.Creator.UserName,
                    PorjectVisibility = projectModel.PorjectVisibility,
                    InviteCode = projectModel.InviteCode,
                    StartedAt = projectModel.StartedAt,
                    EstComplete = projectModel.EstComplete,
                    ActualComplete = projectModel.ActualComplete,
                    CreatedAt = projectModel.CreatedAt,
                    UpdatedAt = projectModel.UpdatedAt,
                    UserDetails = userDetails,
                };
                return projectDetail;
            }
            catch{
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }

        }
        public async Task<List<UserProjectDetailsDto>> GetUserProjectDetails(ProjectModel model)
        {
            return model.UserProjects.Select(u => new UserProjectDetailsDto
            {
                UserId = u.UserId,
                Username = u.User.UserName
            }).ToList();
        }
        private async Task<List<UserProject>> MapUserProject( HashSet<string> Users,Guid projectId) {
            return Users
              .Select(userId => new UserProject
              {
                  ProjectId = projectId,
                  UserId = userId
              }).ToList();
        }
        public async Task DeleteProject(Guid ProejctId) { 
            var userID = _helperService.GetTokenDetails().userId ?? throw new KeyNotFoundException("UserID not found");
            var project = await _projectRepo.GetProjectByID(ProejctId);
            if (project != null)
            {
                throw new KeyNotFoundException("Project cannot be found with this Id");
            }

            if (project.CreatorId != userID) throw new InvalidOperationException("That is not your porject DumbAss"); 
         
            await _projectRepo.DeleteProject(project);
        }
    }
}
