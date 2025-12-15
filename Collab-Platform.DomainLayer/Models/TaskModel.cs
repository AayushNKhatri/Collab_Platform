using Collab_Platform.DomainLayer.EnumsAndOther;
using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.DomainLayer.Models
{
    public class TaskModel
    {
        public Guid TaskId { get; set; }
        [Required]
        public string TaskName { get; set; }
        public string? TaskDesc { get; set; }
        public DateTime? TaskDueDate { get; set; }
        [Required]
        public TaskStatusEnum TaskStatus { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        public ProjectModel Project { get; set; }

        [Required]
        public string TaskLeaderId { get; set; }
        public UserModel TaskLeader { get; set; }
        [Required]
        public string CreatedById { get; set; }
        public UserModel CreatedBy { get; set; }
        
        public ICollection<UserTask> UserTasks { get; set; }
        public ICollection<SubtaskModel> Subtasks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
