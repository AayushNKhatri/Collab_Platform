using Collab_Platform.ApplicationLayer.DTO.TaskDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.EnumsAndOther;
using Collab_Platform.DomainLayer.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class TaskService : ITaskInterface
    {
        public readonly ITaskRepo _taskRepo;
        public readonly IHelperService _helperService;
        public readonly IUnitOfWork _unitOfWork;
        public TaskService(ITaskRepo taskRepo, IHelperService helperService, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepo;
            _helperService = helperService;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateTask(CreateTaskDTO createTask)
        {
            try
            {
                if (createTask == null) throw new ArgumentNullException("Fill the create Task Proprly");
                var TaskCreatorId = _helperService.GetTokenDetails().Item1 ?? throw new KeyNotFoundException("User id not found");
                await _unitOfWork.BeginTranctionAsync();
                var task = new TaskModel
                {
                    TaskId = Guid.NewGuid(),
                    TaskName = createTask.TaskName,
                    TaskDesc = createTask.TaskDesc,
                    TaskDueDate = createTask.TaskDueDate,
                    TaskStatus = createTask.TaskStatus,
                    CreatedById = TaskCreatorId,
                    ProjectId = createTask.ProjectId,
                    TaskLeaderId = createTask.TaskLeaderId,
                    UserTasks = new List<UserTask>(),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                //Make sure User is Added to the Project
                await _taskRepo.CreateTask(task);
                var userTask = new List<UserTask> {
                    new UserTask{
                        TaskId = task.TaskId,
                        UserId = TaskCreatorId,
                    }
                };
                await _taskRepo.addUserToTask(userTask);
                if (createTask.TaskMemberID != null)
                {
                    var taskMember = createTask.TaskMemberID.Distinct().Select(u => new UserTask
                    {
                        TaskId = task.TaskId,
                        UserId = u
                    }).ToList();
                    await _taskRepo.addUserToTask(taskMember);
                }
                var saveResult = await _unitOfWork.SaveChangesAsync();
                if (saveResult <= 0) { 
                    await _unitOfWork.RollBackTranctionAsync();
                    throw new Exception("Task was not saved to db transaction aborted");
                };
                await _unitOfWork.CommitTranctionAsync();
            }
            catch {
                throw;
            }
            
        }

        public async Task DeleteTask(Guid TaskId)
        {
            var taskModel = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task does not exist for this id");
            await _taskRepo.DeleteTask(taskModel);
        }

        public async Task<TaskDetailDto> GetTaskById(Guid TaskId)
        {
            var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task does not exist for this id");
            var taskDetail = new TaskDetailDto
            {
                TaskId = task.TaskId,
                TaskName = task.TaskName,
                TaskDesc = task.TaskDesc,
                TaskDueDate = task.TaskDueDate,
                TaskStatus = task.TaskStatus,
                Project = new ProjectDetail { 
                    ProjectId = task.Project.ProjectId,
                    ProjectName = task.Project.ProjectName,
                },
                TaskLeader = new TaskLeader {
                    LeaderId = task.TaskLeaderId,
                    LeaderName = task.TaskLeader.UserName,
                },
                Creator = new Creator { 
                    UserId = task.CreatedBy.Id,
                    UserName = task.CreatedBy.UserName,
                }
            };
            return taskDetail;
        }

        public async Task<List<TaskDetailDto>> GetTaskByProject(Guid ProjectId)
        {
            var task = await _taskRepo.GetAllTaskByProjectId(ProjectId) ?? throw new KeyNotFoundException("This Project id dont have any task");
            var taskDetail = task.Select(t => new TaskDetailDto { 
                TaskId = t.TaskId,
                TaskName = t.TaskName,
                TaskDesc = t.TaskDesc,
                TaskDueDate = t.TaskDueDate,
                TaskStatus = t.TaskStatus,
                Project = new ProjectDetail { 
                    ProjectId = t.Project.ProjectId,
                    ProjectName = t.Project.ProjectName,
                },
                TaskLeader = new TaskLeader { 
                    LeaderId= t.TaskLeaderId,
                    LeaderName = t.TaskLeader.UserName,
                },
                Creator = new Creator { 
                    UserId = t.CreatedById,
                    UserName = t.CreatedBy.UserName
                }
            }).ToList();
            return taskDetail;
        }

        public async Task<List<TaskDetailDto>> GetTaskByUserID()
        {
            var userId = _helperService.GetTokenDetails().Item1;
            var task = await _taskRepo.GetTaskByUserId(userId) ?? throw new KeyNotFoundException("This user dont have any task assigned");
            var taskDetail = task.Select( t => new TaskDetailDto {
                TaskId = t.TaskId,
                TaskName = t.TaskName,
                TaskDesc = t.TaskDesc,
                TaskDueDate = t.TaskDueDate,
                TaskStatus = t.TaskStatus,
                Project = new ProjectDetail
                {
                    ProjectId = t.Project.ProjectId,
                    ProjectName = t.Project.ProjectName,
                },
                TaskLeader = new TaskLeader
                {
                    LeaderId = t.TaskLeaderId,
                    LeaderName = t.TaskLeader.UserName,
                },
                Creator = new Creator
                {
                    UserId = t.CreatedById,
                    UserName = t.CreatedBy.UserName
                }
            }).ToList();
            return taskDetail;
        }

        public async Task UpdateTask(Guid TaskId, UpdateTaskDto updateTask)
        {
            var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task with the given task id not found");
            task.TaskName = updateTask.TaskName ?? task.TaskName;
            task.TaskDesc = updateTask.TaskDesc ?? task.TaskDesc;
            task.TaskDueDate = updateTask.TaskDueDate ?? task.TaskDueDate;
            if (updateTask.TaskStatus != default(TaskStatusEnum))
            {
                task.TaskStatus = updateTask.TaskStatus;
            }
            task.TaskLeaderId = updateTask.TaskLeaderId ?? task.TaskLeaderId;
            await _unitOfWork.SaveChangesAsync();
        }   
    }
}
