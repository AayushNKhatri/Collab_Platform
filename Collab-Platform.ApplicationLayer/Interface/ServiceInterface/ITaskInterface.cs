using Collab_Platform.ApplicationLayer.DTO.TaskDto;

namespace Collab_Platform.ApplicationLayer.Interface.ServiceInterface
{
    public interface ITaskInterface
    {
        Task CreateTask(CreateTaskDTO createTask);
        Task<TaskDetailDto> GetTaskById(Guid TaskId);
        Task DeleteTask(Guid TaskId);
        Task<List<TaskDetailDto>> GetTaskByProject(Guid ProjectId);
        Task<List<TaskDetailDto>> GetTaskByUserID();
        Task UpdateTask(Guid TaskId, UpdateTaskDto updateTask);
    }
}
