using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collab_Platform.DomainLayer.Models
{
    public class SubtaskModel
    {
        public Guid SubtaskId { get; set; }
        [Required]
        public string SubtaskName { get; set; }
        public DateTime? DueDate { get; set; }
        [Required]
        public Guid TaskId { get; set; }
        public TaskModel Task { get; set; }
        public string CreatedById { get; set; }
        public UserModel CreatedBy { get; set; }
        public string? AssignedToId { get; set; }
        public UserModel? AssignedTo { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
