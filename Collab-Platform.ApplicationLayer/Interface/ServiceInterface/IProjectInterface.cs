using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IProjectInterface
    {
        Task<ProjectModel> CreateProject(CreateProjectDto createProjectDto);
        Task<List<ProjectDetailDto>> GetAllProject();
        Task<ProjectDetailDto> GetProjectById(Guid projectId);
        Task<List<ProjectDetailDto>> GetProjectByUserId();
        Task AddUserToProject(AddUserProjectDto addUserProject);
        Task<List<ProjectDetailDto>> UpdateProject(Guid projectID, UpdateProjectDto updateProject);
    }
}
