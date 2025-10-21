namespace Collab_Platform.ApplicationLayer.DTO.UserDto
{
    public class UserProfileDto
    {
        public string Email { get; set; }
        public bool isEmailConfirmed { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<ProjectOfUser> ProjectOfUser { get; set; }
    }
    public class ProjectOfUser { 
        public string ProjectName { get; set; }
        public Guid ProjectId { get; set; }
    }
}
