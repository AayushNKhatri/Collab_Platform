using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.InfastructureLayer.Database;
using Microsoft.EntityFrameworkCore;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class ProjectRepo : IProjectRepo
    {
        private readonly ApplicationDbContext _db;
        public ProjectRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ProjectModel> CreateProject(ProjectModel project)
        {
            await _db.Projects.AddAsync(project);
            return project;
        }
        public async Task UpdateProject(ProjectModel project)
        {
            _db.Projects.Update(project);
        }
        public async Task DeleteProject(ProjectModel project)
        {
            _db.Projects.Remove(project);
        }
        public async Task<List<ProjectModel>> GetAllProjectByUserID(string userID)
        {
            var project = await _db.Projects.Include(u => u.Creator).Include(u => u.UserProjects).ThenInclude(u => u.User).Where(u => u.CreatorId == userID).ToListAsync();
            return project;
        }
        public async Task<ProjectModel> GetProjectByID(Guid ProjectID)
        {
            var project = await _db.Projects.Include(u => u.UserProjects).ThenInclude(u => u.User).FirstOrDefaultAsync(p=>p.ProjectId == ProjectID);
            return project;
        }
        public async Task addUserToProject(List<UserProject> userProject)
        {
            await _db.UserProjects.AddRangeAsync(userProject);
        }
        public async Task deleteUserProject(List<UserProject> userProjects) {
             _db.UserProjects.RemoveRange(userProjects);
        }
        public async Task<List<ProjectModel>> GetAllProject()
        {
            return await _db.Projects.Include(u => u.UserProjects).ThenInclude(u=>u.User).ToListAsync();
        }

        public async Task<List<UserProject>> GetUserProjectByUserIDs(List<string> userIDs, Guid projectId)
        {
            return await _db.UserProjects
                .Include(u=>u.User)
                .Include(x=>x.Project)
                .Where(u => u.ProjectId == projectId && userIDs.Contains(u.UserId)).ToListAsync();
        }
    }
}
