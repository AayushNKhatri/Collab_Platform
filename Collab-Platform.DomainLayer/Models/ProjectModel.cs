using Collab_Platform.DomainLayer.EnumsAndOther;
using System.ComponentModel.DataAnnotations;


namespace Collab_Platform.DomainLayer.Models
{
    public class ProjectModel
    {
        public Guid ProjectId { get; set; } 
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectDesc { get; set; }
        [Required]
        public string CreatorId { get; set; }
        public UserModel Creator { get; set; }
        [Required]
        public PorjectVisibilityEnum PorjectVisibility { get; set; } = PorjectVisibilityEnum.Private;
        public string? InviteCode { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EstComplete { get; set; }
        public DateTime? ActualComplete { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<TaskModel> Tasks { get; set; }
        public ICollection<CustomRoleModels> CustomRoles { get; set; }



    }
}
