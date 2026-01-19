using Collab_Platform.ApplicationLayer.DTO.ProjectDto;
using Collab_Platform.DomainLayer.Models;

namespace Collab_Platform.ApplicationLayer.DTO.Mapper
{
    public class ProjectMapper
    {
        public static ProjectDetailDto ToProjectDto(ProjectModel project) {
            return new ProjectDetailDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectDesc = project.ProjectDesc,
                CreatorName = project.Creator.UserName,
                PorjectVisibility = project.PorjectVisibility,
                InviteCode = project.InviteCode,
                StartedAt = project.StartedAt,
                EstComplete = project.EstComplete,
                ActualComplete = project.ActualComplete,
                UserDetails = project.UserProjects.Select(u => new UserProjectDetailsDto
                {
                    UserId = u.UserId,
                    Username = u.User.UserName,
                }).ToList()
            };
        }
        public static List<ProjectDetailDto> ToListProjectDTO(List<ProjectModel> projects) {
            var list = projects.Select(project => new ProjectDetailDto{
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                ProjectDesc = project.ProjectDesc,
                CreatorName = project.Creator.UserName,
                PorjectVisibility = project.PorjectVisibility,
                InviteCode = project.InviteCode,
                StartedAt = project.StartedAt,
                EstComplete = project.EstComplete,
                ActualComplete = project.ActualComplete,
                UserDetails = project.UserProjects.Select(u => new UserProjectDetailsDto
                {
                    UserId = u.UserId,
                    Username = u.User.UserName,
                }).ToList()
            }).ToList();
            return list;
        }
    }
}
