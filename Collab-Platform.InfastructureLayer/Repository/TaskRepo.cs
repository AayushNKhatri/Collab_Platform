using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.DomainLayer.Models;
using Collab_Platform.InfastructureLayer.Database;
using Microsoft.EntityFrameworkCore;

namespace Collab_Platform.InfastructureLayer.Repository
{
    public class TaskRepo : ITaskRepo
    {
        private readonly ApplicationDbContext _db;
        public TaskRepo(ApplicationDbContext db) { 
            _db = db;
        }
        public async Task CreateTask(TaskModel taskModel)
        {
            await _db.Tasks.AddAsync(taskModel);
        }

        public async Task DeleteTask(TaskModel taskModel)
        {
            _db.Tasks.Remove(taskModel);
        }

        public async Task<List<TaskModel>> GetAllTaskByProjectId(Guid ProjectId)
        {
            var task = await _db.Tasks.Include(x=>x.Project).Where(x => x.ProjectId == ProjectId).ToListAsync();
            return task;
        }

        public async Task<TaskModel> GetTaskByTaskId(Guid TaskID)
        {
            var task = await _db.Tasks.Include(x=>x.Project).FirstOrDefaultAsync(x=>x.TaskId == TaskID);
            return task;
        }

        public async Task<List<TaskModel>> GetTaskByUserId(string UserID)
        {
            var task = await _db.Tasks.Include(x => x.Project).Where(x => x.CreatedById == UserID).ToListAsync();
            return task;
        }

        public async Task UpdateTask(TaskModel taskModel)
        {
            _db.Tasks.Update(taskModel);
        }
        public async Task addUserToTask(List<UserTask> userTasks) { 
            await _db.UserTask.AddRangeAsync(userTasks);  
        }
        public async Task deleteUserFormTask(List<UserTask> userTasks) {
            _db.UserTask.RemoveRange(userTasks);
        }
    }
}
