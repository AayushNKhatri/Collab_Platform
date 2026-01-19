using Collab_Platform.DomainLayer.EnumsAndOther;

namespace Collab_Platform.ApplicationLayer.DTO.TaskDto
{
    public class TaskDetailDto
    {
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public string? TaskDesc { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public TaskStatusEnum TaskStatus { get; set; }
        public string TaskLeaderName { get; set; }
        public string TaskLeaderId { get; set; }
        public string TaskCreatorName { get; set; }
        public string TaskCreatorId { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<TaskUser> TaskUser { get; set; }
    }
    public class TaskUser { 
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
