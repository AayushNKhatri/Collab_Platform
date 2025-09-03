using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.InfastructureLayer.Database;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class ProjectRepo : IProjectRepo
    {
        private readonly ApplicationDbContext _db;
        public ProjectRepo(ApplicationDbContext db )
        {
            _db = db;
        }
        public async Task<ProjectModel> CreateProject(ProjectModel project)
        {
            try { 
                await _db.Projects.AddAsync(project);
                await _db.SaveChangesAsync();
                return project;
            }
            catch (Exception e) {
                throw new Exception($"Error", e);
            }
        }
    }
}
