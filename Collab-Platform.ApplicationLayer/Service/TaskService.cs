using Collab_Platform.ApplicationLayer.DTO.Mapper;
using Collab_Platform.ApplicationLayer.DTO.TaskDto;
using Collab_Platform.ApplicationLayer.Interface.HelperInterface;
using Collab_Platform.ApplicationLayer.Interface.RepoInterface;
using Collab_Platform.ApplicationLayer.Interface.ServiceInterface;
using Collab_Platform.DomainLayer.Models;
using static Collab_Platform.PresentationLayer.Middleware.ExecptionClass;

namespace Collab_Platform.ApplicationLayer.Service
{
    public class TaskService : ITaskInterface
    {
        public readonly ITaskRepo _taskRepo;
        public readonly IDataHelper _helperService;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IProjectInterface _projectInterface;
        public readonly IProjectRepo _projectRepo;
        public TaskService(ITaskRepo taskRepo, IDataHelper helperService, IUnitOfWork unitOfWork, IProjectInterface projectInterface, IProjectRepo projectRepo)
        {
            _taskRepo = taskRepo;
            _helperService = helperService;
            _unitOfWork = unitOfWork;
            _projectInterface = projectInterface;
            _projectRepo = projectRepo;
        }
        public async Task CreateTask(CreateTaskDTO createTask, Guid ProjectId)
        {

            await _unitOfWork.BeginTranctionAsync();
            try
            {
                if (createTask == null) throw new ArgumentNullException("Fill the create Task Proprly");
                var project = await _projectRepo.GetProjectByID(ProjectId);
                var TaskCreatorId = _helperService.GetTokenDetails().userId ?? throw new KeyNotFoundException("User id not found");
                var task = new TaskModel
                {
                    TaskId = Guid.NewGuid(),
                    TaskName = createTask.TaskName,
                    TaskDesc = createTask.TaskDesc,
                    TaskDueDate = createTask.TaskDueDate,
                    TaskStatus = createTask.TaskStatus,
                    CreatedById = TaskCreatorId,
                    ProjectId = ProjectId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await _taskRepo.CreateTask(task);
                List<string> newUserTask = new List<string>();
                var projectMember = await _projectInterface.GetUserProjectDetails(project) ?? throw new KeyNotFoundException("There is no member in project select member first");
                var projectMemberID = projectMember.Select(m => m.UserId).ToList();
                if (createTask.TaskLeaderId == null)                      //Check if creator added a task leader if not creator is task leader
                {
                    task.TaskLeaderId = TaskCreatorId;
                    newUserTask.Add(task.TaskLeaderId);
                }
                else {
                    if (!projectMemberID.Any(memberId => memberId.Contains(createTask.TaskLeaderId)))               //This is validation for if task leader is project member or not
                    {
                        throw new InvalidOperationException("The task leader is not a porject member");
                    }

                    task.TaskLeaderId = createTask.TaskLeaderId;
                    newUserTask.Add(task.TaskLeaderId);
                }
                if (createTask.TaskMemberID != null) {
                    var taskMemberFormDTO = createTask.TaskMemberID;
                    var notInProject = taskMemberFormDTO.Except(projectMemberID);
                    if (notInProject.Any())
                    {
                        throw new KeyNotFoundException("Task Member IDs are not part of the project");
                    }
                    newUserTask.AddRange(createTask.TaskLeaderId);
                    var invalidUser = createTask.TaskMemberID.Where(id => !projectMemberID.Contains(id)).ToList();
                    if (invalidUser.Count > 0)
                    {
                        await _unitOfWork.RollBackTranctionAsync();
                        throw new ArgumentException("Some users are not part of the project");
                    }
                    newUserTask.Add(TaskCreatorId);
                }
                var userTask = newUserTask.Distinct().Select(u => new UserTask
                {
                    TaskId = task.TaskId,
                    UserId = u
                }).ToList();
                await _taskRepo.addUserToTask(userTask);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTranctionAsync();
            }
            catch
            {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }

        }

        public async Task DeleteTask(Guid TaskId)
        {
            var taskModel = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task does not exist for this id");
            await _taskRepo.DeleteTask(taskModel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TaskDetailDto> GetTaskById(Guid TaskId)
        {
            var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task does not exist for this id");
            var taskDetail = TaskMapper.ToTaskDetailDto(task);
            return taskDetail;
        }

        public async Task<List<TaskDetailDto>> GetTaskByProject(Guid ProjectId)
        {
            var task = await _taskRepo.GetAllTaskByProjectId(ProjectId) ?? throw new KeyNotFoundException("This Project id dont have any task");
            var taskDetail = TaskMapper.ToListTaskDetailDto(task);
            return taskDetail;
        }

        public async Task<List<TaskDetailDto>> GetTaskByUserID()
        {
            var userId = _helperService.GetTokenDetails().userId;
            var task = await _taskRepo.GetTaskByUserId(userId) ?? throw new KeyNotFoundException("This user dont have any task assigned");
            var taskDetail = TaskMapper.ToListTaskDetailDto(task);
            return taskDetail;
        }
        public async Task AddUserToTask(Guid TaskId, List<string> UserId)
        {
            await _unitOfWork.BeginTranctionAsync();
            try
            {
                //var userId = _helperService.GetTokenDetails().userId;
                var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task with the given task id not found");
                var existingUserTasks = task.UserTasks.Select(ut => ut.UserId).ToHashSet();
                var newUserTasks = UserId
                    .Where(userId => !existingUserTasks.Contains(userId))
                    .Select(userId => new UserTask
                    {
                        TaskId = TaskId,
                        UserId = userId
                    }).ToList();
                await _taskRepo.addUserToTask(newUserTasks);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }

        }
        public async Task EditUserFormTask(Guid TaskId, List<string> UserId)
        {
            await _unitOfWork.BeginTranctionAsync();
            try
            {
                var userID = _helperService.GetTokenDetails().userId;
                var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("There is no task found");
                var existingTaskMember = task.UserTasks.Select(u => u.UserId).ToHashSet();
                var newUserToAddHaset = UserId.Where(u => !existingTaskMember.Contains(u) && existingTaskMember.Remove(task.TaskLeaderId)).ToHashSet();
                var userToRemoveList = existingTaskMember;
                userToRemoveList.ExceptWith(newUserToAddHaset);
                if (newUserToAddHaset != null)
                {
                    var newUsetToTask = newUserToAddHaset.Select(u => new UserTask
                    {
                        TaskId = TaskId,
                        UserId = u
                    }).ToList();
                    await _taskRepo.addUserToTask(newUsetToTask);
                }
                if(userToRemoveList != null)
                {
                   var userToRemove = userToRemoveList.Select(u => new UserTask
                    {
                        TaskId = TaskId,
                        UserId = u
                    }).ToList(); 
                    await _taskRepo.deleteUserFormTask(userToRemove);
                }
            }
            catch
            {
                await _unitOfWork.RollBackTranctionAsync();
                throw;
            }
        }
        public async Task RemoveUserFormTask(Guid TaskId, List<string> UserId)  //Validation Left
        {
            await _unitOfWork.BeginTranctionAsync();
            try
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
                await _taskRepo.deleteUserFormTask(userToRemove);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                await _unitOfWork.RollBackTranctionAsync();
            }

        }
        public async Task UpdateTask(Guid TaskId, UpdateTaskDto updateTask)
        {
            await _unitOfWork.BeginTranctionAsync();
            try
            {
                var userId = _helperService.GetTokenDetails().userId;
                var task = await _taskRepo.GetTaskByTaskId(TaskId) ?? throw new KeyNotFoundException("Task with the given task id not found");
                task.TaskName = updateTask.TaskName;
                task.TaskDesc = updateTask.TaskDesc;
                task.TaskDueDate = updateTask.TaskDueDate;
                task.TaskStatus = updateTask.TaskStatus;
                await _unitOfWork.SaveChangesAsync();
                if (task.TaskLeaderId != updateTask.TaskLeaderId)
                {
                    task.TaskLeaderId = updateTask.TaskLeaderId;
                    var taskLeader = task.UserTasks.Where(ut => ut.UserId == task.TaskLeaderId).ToList();
                    await _taskRepo.deleteUserFormTask(taskLeader);
                    var newLeader = new UserTask
                    {
                        UserId = updateTask.TaskLeaderId,
                        TaskId = TaskId
                    };
                    await _taskRepo.addUserToTask(taskLeader);
                    await _unitOfWork.SaveChangesAsync();
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
                    await _unitOfWork.SaveChangesAsync();
                }

                if (memberToRemove.Any())
                {
                    var deleteUser = task.UserTasks.Where(u => memberToRemove.Contains(u.UserId)).ToList();
                    await _taskRepo.deleteUserFormTask(deleteUser);
                    await _unitOfWork.SaveChangesAsync();
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
            var taskDetail = TaskMapper.ToListTaskDetailDto(task);
            return taskDetail;
        }
    }
}
