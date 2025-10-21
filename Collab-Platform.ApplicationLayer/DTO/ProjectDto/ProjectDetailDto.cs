using Collab_Platform.DomainLayer.EnumsAndOther;

namespace Collab_Platform.ApplicationLayer.DTO.ProjectDto
{
    public class ProjectDetailDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public string CreatorName { get; set; }
        public PorjectVisibilityEnum PorjectVisibility { get; set; }
        public string? InviteCode { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EstComplete { get; set; }
        public DateTime? ActualComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<UserProjectDetailsDto> ? UserDetails { get; set; }
    }

    public class UserProjectDetailsDto
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        
    }

}
