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
        public ProjectDetail Project { get; set; }
        public TaskLeader TaskLeader { get; set; }
        public Creator Creator { get; set; }
    }
    public class ProjectDetail { 
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
    public class TaskLeader { 
        public string LeaderId { get; set; }
        public string LeaderName { get; set; }
    }
    public class Creator { 
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
