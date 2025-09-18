using Collab_Platform.DomainLayer.EnumsAndOther;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collab_Platform.ApplicationLayer.DTO.ProjectDto
{
    public class UpdateProjectDto
    {
        public string? ProjectName { get; set; }
        public string? ProjectDesc { get; set; }
        public PorjectVisibilityEnum? PorjectVisibility { get; set; }
        public List<string>? ProjectMemberID { get; set; }
        public DateTime? EstComplete { get; set; }
    }
}
