using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Http;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class ProjectService : IProjectInterface
    {
        private readonly IProjectRepo _projectRepo;
        private readonly IHelperService _helperService;
        public ProjectService(IProjectRepo projectRepo, IHelperService helperService) { 
            _projectRepo = projectRepo;
            _helperService = helperService;
        }

        public async Task<ProjectModel> CreateProject(CreateProjectDto createProjectDto)
        {
            try {
                var userID = _helperService.GetTokenDetails().Item1 ?? throw new KeyNotFoundException("UserID not found");
                var project = new ProjectModel {
                    ProjectId = Guid.NewGuid(),
                    ProjectName = createProjectDto.ProjectName,
                    PorjectVisibility = createProjectDto.PorjectVisibility,
                    ProejctDesc = createProjectDto.ProejctDesc,
                    CreatorId = userID,
                    StartedAt = DateTime.Now,
                    EstComplete = createProjectDto.EstComplete,
                    ActualComplete = null,
                };
                var result = await _projectRepo.CreateProject(project);
                return result;
            }
            catch (Exception e) {
                throw new Exception("Error", e);
            }
        }
    }
}
