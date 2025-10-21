using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface IProjectRepo
    {
        Task<ProjectModel> CreateProject(ProjectModel project);
        Task UpdateProject(ProjectModel project);
        Task DeleteProject(ProjectModel project);
        Task<List<ProjectModel>> GetAllProjectByUserID(string userID);
        Task<ProjectModel> GetProjectByID(Guid ProjectID);
        Task addUserToProject(List<UserProject> userProject);
        Task<List<ProjectModel>> GetAllProject();
        Task deleteUserProject(List<UserProject> userProjects);
    }
}
