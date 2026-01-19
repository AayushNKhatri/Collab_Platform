using Collab_Platform.ApplicationLayer.DTO.TaskDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.DTO.Mapper
{
    public class TaskMapper
    {
        public static TaskDetailDto ToTaskDetailDto(TaskModel task) 
        {
            return new TaskDetailDto { 
                TaskId = task.TaskId,
                TaskName = task.TaskName,
                TaskDesc = task.TaskDesc,
                TaskLeaderName =task.TaskLeader.UserName,
                TaskLeaderId = task.TaskLeaderId,
                TaskDueDate = task.TaskDueDate,
                TaskStatus = task.TaskStatus,
                TaskCreatorId = task.CreatedById,
                TaskCreatorName = task.CreatedBy.UserName,
                ProjectId = task.ProjectId,
                ProjectName = task.Project.ProjectName,
                TaskUser = task.UserTasks.Select(u => new TaskUser { 
                    UserId = u.UserId,
                    UserName = u.User.UserName
                }).ToList()
                
            };
        }
        public static List<TaskDetailDto> ToListTaskDetailDto(List<TaskModel> task)
        {
            var list = task.Select(task => new TaskDetailDto
            {
                TaskId = task.TaskId,
                TaskName = task.TaskName,
                TaskDesc = task.TaskDesc,
                TaskLeaderName = task.TaskLeader.UserName,
                TaskLeaderId = task.TaskLeaderId,
                TaskDueDate = task.TaskDueDate,
                TaskStatus = task.TaskStatus,
                TaskCreatorId = task.CreatedById,
                TaskCreatorName = task.CreatedBy.UserName,
                ProjectId = task.ProjectId,
                ProjectName = task.Project.ProjectName,
                TaskUser = task.UserTasks.Select(u => new TaskUser
                {
                    UserId = u.UserId,
                    UserName = u.User.UserName
                }).ToList()

            }).ToList();
            return list;
        }
    }
}
