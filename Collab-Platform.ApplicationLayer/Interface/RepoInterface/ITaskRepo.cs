using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.Interface.RepoInterface
{
    public interface ITaskRepo
    {
        Task CreateTask(TaskModel taskModel);
        Task DeleteTask(TaskModel taskModel);
        Task UpdateTask(TaskModel taskModel);
        Task<TaskModel> GetTaskByTaskId(Guid TaskID);
        Task<List<TaskModel>> GetTaskByUserId(string UserID);
        Task<List<TaskModel>> GetAllTaskByProjectId(Guid ProjectId);
        Task addUserToTask(List<UserTask> userTasks);
        Task deleteUserFormTask(List<UserTask> userTasks);
        Task<List<TaskModel>> GetTaskByCreator(string UserId);
    } 
}
