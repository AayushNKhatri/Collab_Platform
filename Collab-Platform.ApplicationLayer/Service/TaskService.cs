using Collab_Platform.ApplicationLayer.DTO.TaskDto;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using static Collab_Platform.PresentationLayer.Middleware.ExecptionClass;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class TaskService : ITaskInterface
    {
        public readonly ITaskRepo _taskRepo;
        public readonly IHelperService _helperService;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IProjectInterface _projectInterface;
        public readonly IProjectRepo _projectRepo;
        public TaskService(ITaskRepo taskRepo, IHelperService helperService, IUnitOfWork unitOfWork, IProjectInterface projectInterface, IProjectRepo projectRepo)
        {
            _taskRepo = taskRepo;
            _helperService = helperService;
            _unitOfWork = unitOfWork;
            _projectInterface = projectInterface;
            _projectRepo = projectRepo;
        }
        public async Task CreateTask(CreateTaskDTO createTask)
        {
            try
            {
                if (createTask == null) throw new ArgumentNullException("Fill the create Task Proprly");
                var project = await _projectRepo.GetProjectByID(createTask.ProjectId);
                var TaskCreatorId = _helperService.GetTokenDetails().userId ?? throw new KeyNotFoundException("User id not found");
                if(project.CreatorId != TaskCreatorId) throw new InvalidOperationException("Task Creator id not found");
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
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await _taskRepo.CreateTask(task);
                
                var projectMember = await _projectInterface.GetUserProjectDetails(project) ?? throw new KeyNotFoundException("There is no member in project select member first");
                var projectMemberID = projectMember.Select(m => m.UserId).ToHashSet();
                var taskMemberFormDTO = createTask.TaskMemberID;
                var notInProject = taskMemberFormDTO.Except(projectMemberID);
                if (notInProject.Any())
                {
                    throw new KeyNotFoundException("Task Member IDs are not part of the project");
                }
                var taskMemberIds = createTask.TaskMemberID?.Distinct().ToList() ?? new List<string>();
                var invalidUser = createTask.TaskMemberID.Where(id => !projectMemberID.Contains(id)).ToList();
                if (invalidUser.Count > 0)
                {
                    await _unitOfWork.RollBackTranctionAsync();
                    throw new ArgumentException("Some users are not part of the project");
                }
                taskMemberIds.Add(TaskCreatorId);
                taskMemberIds = taskMemberIds.Distinct().ToList();
                var userTask = taskMemberIds.Select(u => new UserTask
                {
                    TaskId = task.TaskId,
                    UserId = u
                }).ToList();
                await _taskRepo.addUserToTask(userTask);
                var saveResult = await _unitOfWork.SaveChangesAsync();
                if (saveResult <= 0) { 
                    await _unitOfWork.RollBackTranctionAsync();
                    throw new Exception("Task was not saved to db transaction aborted");
                };
                await _unitOfWork.CommitTranctionAsync();
            }
            catch {
                await _unitOfWork.RollBackTranctionAsync();
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
            var userId = _helperService.GetTokenDetails().userId;
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
        public async Task AddUserToTask(Guid TaskId, List<string> UserId)  //Validation Left
        {
            var userId = _helperService.GetTokenDetails().userId;
            var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task with the given task id not found");
            if (userId != task.TaskLeaderId || userId != task.Project.CreatorId) {
                throw new InvalidOperationException("This is not valid");
            }
            var existingUserTasks = task.UserTasks.Select(ut => ut.UserId).ToHashSet();
            var newUserTasks = UserId
                .Where(userId => !existingUserTasks.Contains(userId))
                .Select(userId => new UserTask
                {
                    TaskId = TaskId,
                    UserId = userId
                }).ToList();
            _taskRepo.addUserToTask(newUserTasks);
        }
        public async Task RemoveUserFormTask(Guid TaskId, List<string> UserId)  //Validation Left
        {
            var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task with give task is not found");
            var existingUserTask = task.UserTasks.Select(ut => ut.UserId).ToHashSet();
            var userToRemove = UserId
                .Where(userid => !existingUserTask.Contains(userid))
                .Select(userid => new UserTask
                {
                    TaskId = TaskId,
                    UserId = userid
                }).ToList();
            _taskRepo.deleteUserFormTask(userToRemove);
        }

        public async Task UpdateTask(Guid TaskId, UpdateTaskDto updateTask)
        {
            await _unitOfWork.BeginTranctionAsync();
            try
            {
                var userId = _helperService.GetTokenDetails().userId;
                var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task with the given task id not found");
                if (task.TaskLeaderId != userId || task.Project.CreatorId != userId)
                {
                    throw new InvalidRoleException("You must be the creator of this task or creator of the project");
                }
                task.TaskName = updateTask.TaskName;
                task.TaskDesc = updateTask.TaskDesc;
                task.TaskDueDate = updateTask.TaskDueDate;
                task.TaskStatus = updateTask.TaskStatus;
                if (task.TaskLeaderId != updateTask.TaskLeaderId)
                {
                    task.TaskLeaderId = updateTask.TaskLeaderId;
                    var taskLeader = task.UserTasks.Where(ut => ut.UserId == task.TaskLeaderId).ToList();
                    await _taskRepo.deleteUserFormTask(taskLeader);
                    var newLeader = new UserTask
                    {
                        UserId =  updateTask.TaskLeaderId,
                        TaskId = TaskId
                    };
                    await _taskRepo.addUserToTask(taskLeader);
                }
                var existingTaskMember = task.UserTasks
                        .Where(u => u.UserId == task.CreatedById)
                        .Select(u => u.UserId).ToHashSet();
                var incomingUser = updateTask.TaskMembers.ToHashSet();
                var memberToRemove = existingTaskMember.Except(incomingUser).ToHashSet();
                incomingUser.Except(memberToRemove);
                if (incomingUser.Any())
                {
                    var addMember = incomingUser.Select(userId => new UserTask
                    {
                        UserId = userId,
                        TaskId = TaskId,
                    }).ToList();
                    await _taskRepo.addUserToTask(addMember);
                }

                if (memberToRemove.Any())
                {
                    var deleteUser = task.UserTasks.Where(u => memberToRemove.Contains(u.UserId)).ToList();
                    await _taskRepo.deleteUserFormTask(deleteUser);
                }
                
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }

           
        }

        public async Task<List<TaskDetailDto>> GetTasksByCreatorId()
        {
            var UserID = _helperService.GetTokenDetails().userId;
            var task = await _taskRepo.GetTaskByCreator(UserID) ?? throw new KeyNotFoundException("This user dont have any task");
            var taskDetail = task.Select( t => new TaskDetailDto
            {
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
    }
}
