using Collab_Platform.DomainLayer.EnumsAndOther;

namespace Collab_Platform.ApplicationLayer.DTO.TaskDto
{
    public class UpdateTaskDto
    {
        public string TaskName { get; set; }
        public string? TaskDesc { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public TaskStatusEnum TaskStatus { get; set; }
        public string TaskLeaderId { get; set; }
    }
}
