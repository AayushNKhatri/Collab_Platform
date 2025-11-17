using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface IProjectInterface
    {
        Task<ProjectModel> CreateProject(CreateProjectDto createProjectDto);
        Task<List<ProjectDetailDto>> GetAllProject();
        Task<List<UserProjectDetailsDto>> GetUserProjectDetails(ProjectModel model);
        Task<ProjectDetailDto> GetProjectById(Guid projectId);
        Task<List<ProjectDetailDto>> GetProjectByUserId();
        Task AddUserToProject(Guid projectID, List<string> UserId);
        Task<ProjectDetailDto> UpdateProject(Guid projectID, UpdateProjectDto updateProject);
        Task DeleteProject(Guid ProjectId);
        Task RemoveUserFormProject(Guid projectId, List<string> UserID);
    }
}
