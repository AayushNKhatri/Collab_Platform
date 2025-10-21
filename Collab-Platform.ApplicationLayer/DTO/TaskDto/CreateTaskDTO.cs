﻿using Collab_Platform.DomainLayer.EnumsAndOther;

namespace Collab_Platform.ApplicationLayer.DTO.TaskDto
{
    public class CreateTaskDTO
    {
        public Guid ProjectId { get; set; }
        public string TaskName { get; set; }
        public string? TaskDesc { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public TaskStatusEnum TaskStatus { get; set; }
        public string TaskLeaderId { get; set; }
        public List<string>? TaskMemberID { get; set; }
    }
}
