using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface IProjectRepo
    {
        Task<ProjectModel> CreateProject(ProjectModel project);
    }
}
