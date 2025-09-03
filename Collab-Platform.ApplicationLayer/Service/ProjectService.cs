using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class ProjectService : IProjectInterface
    {
        private readonly IProjectRepo _projectRepo;
        private readonly IUnitOfWork _unitOfWok;
        public ProjectService(IProjectRepo projectRepo, IUnitOfWork unitOfWok) { 
            _projectRepo = projectRepo;
            _unitOfWok = unitOfWok;
        }

        public async Task<CustomResult<ProjectModel, string>> CreateProject(CreateProjectDto createProjectDto)
        {
            await _unitOfWok.BeginTranctionAsync();
            try {
                var project = new ProjectModel {
                    ProjectId = Guid.NewGuid(),
                    ProjectName = createProjectDto.ProjectName,
                    PorjectVisibility = createProjectDto.PorjectVisibility,
                    ProejctDesc = createProjectDto.ProejctDesc,
                    CreatorId = "SomeString",
                    StartedAt = DateTime.Now,
                    EstComplete = createProjectDto.EstComplete,
                    ActualComplete = null,
                };
                await _projectRepo.CreateProject(project);
                await _unitOfWok.CommitTranctionAsync();
                return CustomResult<ProjectModel, string>.Ok(project, "Project has been created");
            }
            catch (Exception e) {
                await _unitOfWok.RollBackTranctionAsync();
                return CustomResult<ProjectModel, string>.Fail(new List<string> {e.Message}, "Failed to create project");
            }
        }
    }
}
