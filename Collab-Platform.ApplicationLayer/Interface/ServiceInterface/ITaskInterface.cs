using Collab_Platform.ApplicationLayer.DTO.TaskDto;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface ITaskInterface
    {
        Task CreateTask(CreateTaskDTO createTask, Guid ProjectId);
        Task<TaskDetailDto> GetTaskById(Guid TaskId);
        Task DeleteTask(Guid TaskId);
        Task<List<TaskDetailDto>> GetTaskByProject(Guid ProjectId);
        Task<List<TaskDetailDto>> GetTaskByUserID();
        Task UpdateTask(Guid TaskId, UpdateTaskDto updateTask);
        Task<List<TaskDetailDto>> GetTasksByCreatorId();
        Task AddUserToTask(Guid TaskId, List<string> UserId);
        Task RemoveUserFormTask(Guid TaskId, List<string> UserId);
        Task EditUserFormTask(Guid TaskId, List<string> UserId);

    }
}
