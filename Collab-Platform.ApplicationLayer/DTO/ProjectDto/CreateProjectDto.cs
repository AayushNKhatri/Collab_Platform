using Collab_Platform.DomainLayer.EnumsAndOther;
using Collab_Platform.DomainLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace Collab_Platform.ApplicationLayer.DTO.ProjectDto
{
    public class CreateProjectDto
    {
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string ProjectDesc { get; set; }
        [Required]
        public PorjectVisibilityEnum PorjectVisibility { get; set; }
        public List<string> ProjectMemberID { get; set; }
        public DateTime? EstComplete { get; set; }
    }
}
